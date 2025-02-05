using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Gift_Card.Models
{
    public class SimpleNotificationService
    {
        private string awsKeyId { get; set; }
        private string awsKeySecret { get; set; }

        public void SendSMS(string number ,string message)
        {
            awsKeyId = "ASIAVOR2H4M4F4JE4F6W";
            awsKeySecret = "X086g8W/665Nc4xZUJXh5KQm6HzsMCIyGZVRXG/2";
            var credentails = new BasicAWSCredentials(awsKeyId, awsKeySecret);
            AmazonSimpleNotificationServiceClient client = new AmazonSimpleNotificationServiceClient(credentails);
           

            var request = new PublishRequest
            {
                Message = message,
                PhoneNumber = number
            };

            try
            {
                var response = client.Publish(request);
            }
            catch (Exception ex)
            {

            }
        }
    }
}