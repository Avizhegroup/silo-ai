namespace SiloAI.Shared.Tools;

/// <summary>
/// SQL Server text processing tools used by the AI agent layer to parse and
/// extract SQL commands embedded in AI-generated responses.
/// </summary>
public static class SqlTextTools
{
    private const string SqlStartPattern = "<<SQL";
    private const string SqlEndPattern = ">>";

    /// <summary>
    /// Strips all embedded SQL blocks (delimited by <c>&lt;&lt;SQL … &gt;&gt;</c>) from
    /// <paramref name="text"/>. Optionally collects the extracted SQL command strings into
    /// <paramref name="collectedCommands"/>.
    /// </summary>
    /// <param name="text">The raw AI response text that may contain SQL blocks.</param>
    /// <param name="collectedCommands">
    /// When provided, each extracted SQL command body is appended to this list.
    /// Pass <c>null</c> to discard the extracted commands.
    /// </param>
    /// <returns>The cleaned text with all SQL blocks removed and trimmed.</returns>
    public static string StripSqlBlocks(string text, List<string>? collectedCommands = null)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var startIndex = 0;

        while (startIndex < text.Length)
        {
            var sqlStartIndex = text.IndexOf(SqlStartPattern, startIndex, StringComparison.OrdinalIgnoreCase);
            if (sqlStartIndex == -1)
                break;

            var sqlEndIndex = text.IndexOf(SqlEndPattern, sqlStartIndex + SqlStartPattern.Length, StringComparison.OrdinalIgnoreCase);
            if (sqlEndIndex == -1)
                break;

            var sqlContent = text
                .Substring(sqlStartIndex + SqlStartPattern.Length, sqlEndIndex - (sqlStartIndex + SqlStartPattern.Length))
                .Trim();

            if (collectedCommands is not null && sqlContent.Length > 0)
                collectedCommands.Add(sqlContent);

            var blockLength = (sqlEndIndex + SqlEndPattern.Length) - sqlStartIndex;
            text = text.Remove(sqlStartIndex, blockLength);
            startIndex = sqlStartIndex;
        }

        return text.Trim();
    }
}
