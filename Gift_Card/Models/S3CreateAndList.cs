using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gift_Card.Models
{
    public class S3CreateAndList
    {
        private string awsKeyId { get; set; }
        private string awsKeySecret { get; set; }
        private string bucketName { get; set; }

        public void UploadFileAws(string filePath)
        {
            awsKeyId = "ASIAVOR2H4M4F4JE4F6W";
            awsKeySecret = "X086g8W/665Nc4xZUJXh5KQm6HzsMCIyGZVRXG/2";
            bucketName = "giftcardstore";
            var client =new AmazonS3Client(awsKeyId, awsKeySecret, bucketName);
            var filetransfer = new TransferUtility(client);
            try
            {
                var req = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456,
                    Key = "SimpleAudio.wav",
                    CannedACL = S3CannedACL.PublicRead,
                };
            }
            catch (Exception ex)
            {

            }
        }
    }
}