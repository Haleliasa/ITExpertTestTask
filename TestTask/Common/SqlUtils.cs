using System;
using System.Collections.Generic;

public static class SqlUtils
{
    /// <summary>
    /// Removes all but SELECT statements from an SQL string
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static string SelectOnly(string sql)
    {
        string[] statements = sql.Split(';');
        List<string> selectOnlyStatements = new List<string>();
        foreach (string statement in statements)
        {
            string statementClean = statement.Trim();
            if (statementClean.StartsWith(
                "select",
                StringComparison.InvariantCultureIgnoreCase))
            {
                selectOnlyStatements.Add(statementClean);
            }
        }
        if (selectOnlyStatements.Count == 0)
        {
            return null;
        }
        sql = string.Join(";\n", selectOnlyStatements);
        return sql;
    }
}
