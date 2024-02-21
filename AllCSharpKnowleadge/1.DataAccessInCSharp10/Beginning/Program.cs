using _1.DataAccessInCSharp10.Service.Command;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Warehouse.Data.SQLite;

using CustomerSqlite = Warehouse.Data.SQLite.Customer;

namespace _1.DataAccessInCSharp10
{
    public class Program
    {
        static void Main(string[] args)
        {
            //1. Get Data from LocalDB
            //Command command = new LocalDbCommand();
            //command.GetData();

            //2. Get data from SQlite
            //Command command = new SqlitesCommand();
            //command.GetData();

            //3. connect to DB through DB Context with SQLserver
            //using var context = new WarehouseContext();

            //SelectData(context);
            //CreateData(context);
            //UpdateData(context);
            //RemoveData(context);

            //4. connect to DB through DB Context with SQLite
            //using var contextSql = new WarehouseSQLiteContext();
            //CreateDataSqlite(contextSql);

            //5. Query data using ADO.NET SQL server
            Console.WriteLine("Pick the action you want to work(1: select, 2: insert: 3: delete");
            var pick = int.Parse(Console.ReadLine());
            QueryByAdonet(pick);
        }

        private static void QueryByAdonet(int pick)
        {
            var connectionString = @"Data Source=DESKTOP-J76PCRA\SQLEXPRESS;Initial Catalog=WAREHOUSEMANAGEMENT;Integrated Security=True";

            using SqlConnection sqlConnection = new SqlConnection(connectionString);

            // choose type of query SQL: select insert delete
            var cmdAction = ActionWithAdo(pick);

            using SqlCommand command = new(cmdAction.queryText , sqlConnection);

            if (pick == 2)
            {
                var nameParameter = new SqlParameter("Name", System.Data.SqlDbType.NVarChar);
                nameParameter.Value = cmdAction.Name;
                command.Parameters.Add(nameParameter);

                var addressParameter = new SqlParameter("Address", System.Data.SqlDbType.NVarChar);
                addressParameter.Value = cmdAction.Address;
                command.Parameters.Add(addressParameter);

                var postalCodeParameter = new SqlParameter("PostalCode", System.Data.SqlDbType.NVarChar);
                postalCodeParameter.Value = cmdAction.PostalCode;
                command.Parameters.Add(postalCodeParameter);

                var countryParameter = new SqlParameter("Country", System.Data.SqlDbType.NVarChar);
                countryParameter.Value = cmdAction.Country;
                command.Parameters.Add(countryParameter);

                var phoneParameter = new SqlParameter("PhoneNumber", System.Data.SqlDbType.NVarChar);
                phoneParameter.Value = cmdAction.PhoneNumber;
                command.Parameters.Add(phoneParameter);
            }
            else if (pick ==3)
            {
                var phoneParameter = new SqlParameter("Id", System.Data.SqlDbType.NVarChar);
                phoneParameter.Value = cmdAction.Id;
                command.Parameters.Add(phoneParameter);
            }
            
            sqlConnection.Open();

            if (pick == 1 ) {
                using var reader = command.ExecuteReader();
                {
                    if (!reader.HasRows) { return; }
                    while (reader.Read())
                    {
                        var orderId = reader["Id"];
                        var customerName = reader["Name"];

                        Console.WriteLine($"Order Id: {orderId}");
                        Console.WriteLine($"Customer Name: {customerName}");
                    }
                }
            }
            else
            {
                var rowAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Rows affected: {rowAffected}");
            }

            sqlConnection.Dispose();
        }

        private static ActionWithAdoModel ActionWithAdo(int pick)
        {
            ActionWithAdoModel model= new();

            if (pick == 1)
            {
                model.queryText = "SELECT * FROM [Orders] JOIN [CUSTOMERS] ON [Customers].Id = [Orders].CustomerId";
            }
            else if (pick == 2)
            {
                model.queryText = @"INSERT INTO [Customers](Id, Name, Address, PostalCode, Country, PhoneNumber)" +
                    " VALUES(NEWID(), @Name, @Address, @PostalCode, @Country, @PhoneNumber)";
                model.Name = "Hoang";
                model.Address = "41 tho nhuom";
                model.PhoneNumber = "1234567890";
                model.PostalCode = "111111";
                model.Country = "Vietnam";
            }
            else
            {
                model.queryText = "DELETE FROM [Customers] WHERE [Id] = @Id";
                model.Id = "B45B08B7-D9A4-4B23-3924-08DC2E006E62";
            }

            return model;
        }

        private static void RemoveData(WarehouseContext context)
        {
            var toDelete = context.Customers.First(cus => cus.Name == "Trang");
            context.Customers.Remove(toDelete);
            context.SaveChanges();
            Console.WriteLine("Remove data sucessfully!");
        }

        private static void UpdateData(WarehouseContext context)
        {
            var toUpdate = context.Customers.First(cus => cus.Name == "Huy");
            toUpdate.Name = "Trang";
            context.Customers.Update(toUpdate);
            context.SaveChanges();
        }

        private static void CreateData(WarehouseContext context)
        {
            Console.WriteLine("Enter Customer Name: ");
            var newCustomer = new Customer
            {
                Name = Console.ReadLine(),
                Address = "Tho Nhuom",
                PostalCode = "100000",
                Country = "Vietnam",
                PhoneNumber = "+84 12345656"
            };

            context.Customers.Add(newCustomer);
            
            context.SaveChanges();
            Console.WriteLine("Create data sucessfully!");
        }

        //private static void CreateDataSqlite(WarehouseSQLiteContext context)
        //{
        //    Console.WriteLine("Enter Customer Name: ");
        //    var newCustomer = new CustomerSqlite
        //    {
        //        Name = Console.ReadLine(),
        //        Address = "Tho Nhuom",
        //        PostalCode = "100000",
        //        Country = "Vietnam",
        //        PhoneNumber = "+84 12345656"
        //    };

        //    context.Customers.Add(newCustomer);

        //    context.SaveChanges();
        //    Console.WriteLine("Create data sucessfully!");
        //}

        private static void SelectData(WarehouseContext context)
        {
            foreach (var order in context.Orders
                .Where(order => order.Customer.Name == "Filip Ekberg")
                .Include(ord => ord.Customer)
                .Include(ord => ord.ShippingProvider)
                .Include(ord => ord.LineItems)
                .ThenInclude(lineItem => lineItem.Item)
                )
            {
                Console.WriteLine($"Order Id : {order.Id}");
                Console.WriteLine($"Customer: {order.Customer.Name}");
                Console.WriteLine($"Shipping Provider: {order.ShippingProvider.Name}");
                foreach (var lineItem in order.LineItems)
                {
                    Console.WriteLine($"Id Line Item: {lineItem.Id} and Quantity : {lineItem.Quantity}");
                    Console.WriteLine($"Item Name : {lineItem.Item.Name} - Description: {lineItem.Item.Description} - Price: {lineItem.Item.Price}");
                }

                Console.WriteLine("---------------------------");
            }
        }
    }
}