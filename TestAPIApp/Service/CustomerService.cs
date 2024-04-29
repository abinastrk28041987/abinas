using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using TestAPIApp.Model;
using static System.Reflection.Metadata.BlobBuilder;

namespace TestAPIApp.Service
{
    public class CustomerService
    {
        private readonly string _connectionString;
        public CustomerService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Model.CustomerOutputModel getCustomerOrders(Model.CustomerInputModel customerInput )
        {
            CustomerOutputModel customerOutputModel = new CustomerOutputModel();
            

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string Customerquery = "SELECT * FROM CUSTOMERS where CUSTOMERID = '"+ customerInput.CustomerId + "' AND EMAIL='" + customerInput.User +"'";

                SqlCommand command = new SqlCommand(Customerquery, connection);
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    customerOutputModel.Customer = new Customer
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1)
                    };
                    }
                }

                string Orderquery = "SELECT ORD.ORDERID,ORD.ORDERDATE,ORD.DELIVERYEXPECTED,C.HOUSENO + C.STREET AS ADDRESS FROM ORDERS ORD INNER JOIN CUSTOMERS C ON C.CUSTOMERID=ORD.CUSTOMERID where ORD.CUSTOMERID = '" + customerInput.CustomerId + "'";
                string orderItemquery = ""; 
                command = new SqlCommand(Orderquery, connection);
                SqlConnection connectionforOrderitems = new SqlConnection(_connectionString);
                connectionforOrderitems.Open();
                
                customerOutputModel.Order = new List<Order>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<OrderItems> ordItem = new List<OrderItems>();
                        orderItemquery = "SELECT p.Productname,quantity,price FROM ORDERITEMS o inner join products p on p.productid=o.productid WHERE o.ORDERID =" + reader.GetInt32(0);
                        SqlCommand commandfororderitems = new SqlCommand(orderItemquery, connectionforOrderitems);
                        using (SqlDataReader readerfororderitems = commandfororderitems.ExecuteReader())
                        {
                            while (readerfororderitems.Read())
                            {
                                OrderItems orderitem = new OrderItems
                                {
                                    Product = readerfororderitems.GetString(0),
                                    Quantity = readerfororderitems.GetInt32(1),
                                    Price = readerfororderitems.GetDecimal(2)
                                };
                                ordItem.Add(orderitem);

                            }
                        }

                        Order order = new Order
                        {
                            OrderId = reader.GetInt32(0),
                            OrderDate = reader.GetDateTime(1),
                            DeliveryDate = reader.GetDateTime(2),
                            DeliveryAddress = reader.GetString(3),
                            OrderItems = ordItem

                        };

                        
                        customerOutputModel.Order.Add(order);

                    }
                }
            }
            return customerOutputModel;

        }
    }
}
