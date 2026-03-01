using System.Text;
using Application.Features.Users.Services;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.UpdateMyProfile;

public class UpdateMyProfileHandler : IRequestHandler<UpdateMyProfileCommand, UpdateMyProfileResult>
{
    private readonly IUserWriteService _writeService;

    public UpdateMyProfileHandler(IUserWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<UpdateMyProfileResult> Handle(UpdateMyProfileCommand request, CancellationToken ct)
    {
        // Normalize in the handler (pure logic), persist via write service
        string? firstName = null;
        if (request.HasFirstName)
            firstName = NormalizeNameOrNull(request.FirstName ?? "");

        string? lastName = null;
        if (request.HasLastName)
            lastName = NormalizeNameOrNull(request.LastName ?? "");

        string? phone = null;
        if (request.HasPhone)
            phone = NormalizeUsPhoneOrNull(request.Phone ?? "");

        var user = await _writeService.UpdateMyProfileAsync(
            request.UserId,
            request.HasFirstName, firstName,
            request.HasLastName, lastName,
            request.HasPhone, phone,
            ct);

        return new UpdateMyProfileResult(
            user.UserId,
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            user.Phone
        );
    }

    private static string? NormalizeNameOrNull(string input)
    {
        var trimmed = CollapseSpaces(input).Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return null;

        var words = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length; i++)
            words[i] = TitleCaseWord(words[i]);

        return string.Join(" ", words);
    }

    private static string TitleCaseWord(string word)
    {
        var parts = SplitKeepSeparators(word, new[] { '-', '\'' });

        for (int i = 0; i < parts.Count; i++)
        {
            var part = parts[i];
            if (part == "-" || part == "'") continue;
            parts[i] = Capitalize(part);
        }

        return string.Concat(parts);
    }

    private static string Capitalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;

        var lower = s.ToLowerInvariant();
        if (lower.Length == 1) return lower.ToUpperInvariant();

        return char.ToUpperInvariant(lower[0]) + lower.Substring(1);
    }

    private static List<string> SplitKeepSeparators(string input, char[] seps)
    {
        var list = new List<string>();
        var sb = new StringBuilder();

        foreach (var ch in input)
        {
            if (seps.Contains(ch))
            {
                if (sb.Length > 0)
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                }
                list.Add(ch.ToString());
            }
            else
            {
                sb.Append(ch);
            }
        }

        if (sb.Length > 0)
            list.Add(sb.ToString());

        return list;
    }

    private static string CollapseSpaces(string input)
    {
        var sb = new StringBuilder(input.Length);
        bool prevSpace = false;

        foreach (var ch in input)
        {
            var isSpace = char.IsWhiteSpace(ch);
            if (isSpace)
            {
                if (!prevSpace)
                    sb.Append(' ');
                prevSpace = true;
            }
            else
            {
                sb.Append(ch);
                prevSpace = false;
            }
        }

        return sb.ToString();
    }

    private static string? NormalizeUsPhoneOrNull(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var digits = new string(input.Where(char.IsDigit).ToArray());

        if (digits.Length == 11 && digits.StartsWith("1"))
            digits = digits.Substring(1);

        if (digits.Length != 10)
            throw new ArgumentException("Phone must be a valid 10-digit US number.");

        var area = digits.Substring(0, 3);
        var mid = digits.Substring(3, 3);
        var last = digits.Substring(6, 4);

        return $"({area}) {mid}-{last}";
    }
}