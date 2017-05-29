using RiakClient;
using RiakClient.Commands.TS;
using RiakClient.Config;
using System;

namespace WebApiLincora.Helpers
{
    public class RiakConnectorHelper : IConnectorHelper
    {
        readonly IRiakClient _client;
        public RiakConnectorHelper(string HostName, string HostIp)
        {
            RiakClusterConfiguration config = new RiakClusterConfiguration()
            {
                Authentication = new RiakAuthenticationConfiguration()
                {
                    //Username = "user",
                    //Password = "Test1234"
                }
            };
            config.AddNode(new RiakNodeConfiguration()
            {
                Name = HostName,
                HostAddress = HostIp
            });
            IRiakEndPoint cluster = new RiakCluster(config);
             _client = cluster.CreateClient();
        }

        /// <summary>
        /// Creates table with specific name and parameters
        /// </summary>
        /// <param name="Table">Table's name written in CamelCase</param>
        /// <param name="Columns">Table columns written in SQL format with PRIMARY KEY specified</param>
        /// <returns>Boolean indicating succes or failure</returns>
        private bool CreateTable(string Table, string Columns)
        {
            string sqlFmt = string.Format(@"CREATE TABLE {0} {1}", Table, Columns);
            return _client.Execute(new Query.Builder()
                                            .WithTable(Table)
                                            .WithQuery(sqlFmt)
                                            .Build()).IsSuccess;
        }

        /// <summary>
        /// Queries given table with an SQL query string provided
        /// </summary>
        /// <param name="table">Table's name written in CamelCase</param>
        /// <param name="query">SQL command to be executed</param>
        /// <returns>Boolean indicating succes or failure</returns>
        public QueryResponse Query(string table, string query)
        {
            var cmd = new Query.Builder()
                            .WithTable(table)
                            .WithQuery(query)
                            .Build();
            _client.Execute(cmd);
            return cmd.Response;
        }

        /// <summary>
        /// Turns Cell[] into Row
        /// </summary>
        /// <param name="cells">Cells array</param>
        /// <returns>Row containing given cells</returns>
        public Row ToRow(Cell[] cells)
        {
            return new Row(cells);
        }

        /// <summary>
        /// Add row to specific table
        /// </summary>
        /// <param name="Table">Name of the table</param>
        /// <param name="columns">Names of the columns</param>
        /// <param name="rows">Array of Rows containing cells to be inserted</param>
        /// <returns></returns>
        public bool AddRows(string Table, Column[] columns, Row[] rows)
        {
            var cmd = _client.Execute(new Store.Builder()
                                            .WithTable(Table)
                                            .WithColumns(columns)
                                            .WithRows(rows)
                                            .Build());
            return cmd.IsSuccess;
        }


        /// <summary>
        /// Deletes entire row from the table
        /// </summary>
        /// <param name="Table">Name of the table</param>
        /// <param name="key">Row with keys to be deleted</param>
        /// <returns></returns>
        public bool DeleteCells(string Table, Row key)
        {
            return _client.Execute(new Delete.Builder()
                                            .WithTable(Table)
                                            .WithKey(key)
                                            .Build()).IsSuccess;
        }

        public bool AddUser(string username, string password)
        {
            string query = string.Format($"INSERT INTO Users (id, username, password) VALUES ({Guid.NewGuid()}, {0}, {1});", username, password);
            return _client.Execute(new Query.Builder()
                        .WithTable("Users")
                        .WithQuery(query)
                        .Build()).IsSuccess;
        }

        public bool RemoveUser(string username)
        {
            return _client.Execute(new Delete.Builder()
                .WithTable("Users")
                .WithKey(new Row(new Cell[] { new Cell(username) }))
                .Build()).IsSuccess;
        }

        public bool ChangePassword(string username, string password, string newpassword)
        {
            //RemoveUser(username);
            //AddUser(username, newpassword);
            throw new NotImplementedException();
        }

        public bool AddSubscriptionTopics(string topic)
        {
            string query = string.Format($"INSERT INTO SubscriptionTopics (topic) VALUES ({0});", topic);
            return _client.Execute(new Query.Builder()
                        .WithTable("Topics")
                        .WithQuery(query)
                        .Build()).IsSuccess;
        }

        public bool AddPublicationTopics(string topic)
        {
            string query = string.Format($"INSERT INTO PublicationTopics (topic) VALUES ({0});", topic);
            return _client.Execute(new Query.Builder()
                        .WithTable("Topics")
                        .WithQuery(query)
                        .Build()).IsSuccess;
        }
    }
}
