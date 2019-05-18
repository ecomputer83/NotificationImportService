using NotificationImportService.DataObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationImportService.Service
{
    public class DataService
    {
        public static void RecordNotification(NotificationValue value)
        {
            var DeviceId = GetDeviceId(value.PhoneNo);
            InsertNotification(DeviceId, value.Client, value.SubjectLine, value.BodyLine);
        }

        private static string GetDeviceId(string phoneNumber)
        {
            var result = "";
            var commandText = "select m.PushNotificationId as DeviceId from Users u right join UserMobileDevice um on u.UserId = um.UserId right join MobileDevice m on um.MobileDeviceId = m.MobileDeviceId where UserName like '%@No%'";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DalexDataContext"].ConnectionString);
            con.Open();
            SqlCommand command = new SqlCommand(commandText, con);
            command.Parameters.AddWithValue("@No", phoneNumber);
            var reader = command.ExecuteReader(System.Data.CommandBehavior.SingleResult);
            while (reader.Read())
            {
                result = reader["DeviceId"].ToString();
            }

            return result;
        }

        private static void InsertNotification(string DeviceId, string ClientNo, string Title, string Body)
        {
            var commandText = "insert into Notification value (NewID(), @No, @DeviceID, 'dalexfda', @Title, @Body, @isSent, @SentDate, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy)";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DalexDataContext"].ConnectionString);
            con.Open();
            SqlCommand command = new SqlCommand(commandText, con);
            command.Parameters.AddWithValue("@No", ClientNo);
            command.Parameters.AddWithValue("@DeviceID", DeviceId);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Body", Body);

            if (string.IsNullOrEmpty(DeviceId))
            {
                command.Parameters.AddWithValue("@isSent", 1);
                command.Parameters.AddWithValue("@SentDate", DateTime.Now);
            }
            else
            {
                command.Parameters.AddWithValue("@isSent", 0);
                command.Parameters.AddWithValue("@SentDate", DBNull.Value);
            }
            command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
            command.Parameters.AddWithValue("@CreatedBy", "SYSTEM");
            command.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
            command.Parameters.AddWithValue("@ModifiedBy", "SYSTEM");
            command.ExecuteNonQuery();
        }
    }
}
