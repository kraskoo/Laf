namespace LafAPI.Data
{
    using System;

    using LafAPI.Data.Common;

    using Microsoft.EntityFrameworkCore;

    public class DbQueryRunner : IDbQueryRunner
    {
        public DbQueryRunner(LafContext context) =>
            this.Context = context ?? throw new ArgumentNullException(nameof(context));

        public LafContext Context { get; set; }

        public void RunQuery(string query, params object[] parameters) =>
            this.Context.Database.ExecuteSqlRaw(query, parameters);

        public void Dispose() =>
            this.Context?.Dispose();
    }
}