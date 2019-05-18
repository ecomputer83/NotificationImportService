namespace NotificationImportService.DataObject
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class NotificationResponse
    {

        [JsonProperty("value")]
        public List<NotificationValue> Value { get; set; }
    }

    public partial class NotificationValue
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("Entry_No")]
        public long EntryNo { get; set; }

        [JsonProperty("Document_Type")]
        public string DocumentType { get; set; }

        [JsonProperty("Document_No")]
        public string DocumentNo { get; set; }

        [JsonProperty("Client")]
        public string Client { get; set; }

        [JsonProperty("Triggered_By_Record")]
        public string TriggeredByRecord { get; set; }

        [JsonProperty("Sent")]
        public bool Sent { get; set; }

        [JsonProperty("To_Address")]
        public string ToAddress { get; set; }

        [JsonProperty("PhoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty("Copy_to_Address")]
        public string CopyToAddress { get; set; }

        [JsonProperty("Subject_Line")]
        public string SubjectLine { get; set; }

        [JsonProperty("Body_Line")]
        public string BodyLine { get; set; }

        [JsonProperty("Attachment_Filename")]
        public string AttachmentFilename { get; set; }

        [JsonProperty("Sending_Date")]
        public DateTimeOffset SendingDate { get; set; }

        [JsonProperty("Sending_Time")]
        public string SendingTime { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Error_Text")]
        public string ErrorText { get; set; }

        [JsonProperty("Created_By")]
        public string CreatedBy { get; set; }

        [JsonProperty("Created_Datetime")]
        public DateTimeOffset CreatedDatetime { get; set; }

        [JsonProperty("Last_Modified_By")]
        public string LastModifiedBy { get; set; }

        [JsonProperty("Last_Modified_Datetime")]
        public DateTimeOffset LastModifiedDatetime { get; set; }

        [JsonProperty("ETag")]
        public string ETag { get; set; }
    }
}
