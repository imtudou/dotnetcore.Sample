using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Data
{
    public enum DbProviderType:byte
    {
        SqlServer,
        MySql,
        SQLite,
        Oracle,
        ODBC,
        OleDb,
        Firebird,
        PostgreSql,
        DB2,
        Informix,
        SqlServerCe
    }
}
