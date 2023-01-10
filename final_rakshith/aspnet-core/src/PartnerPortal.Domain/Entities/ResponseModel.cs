using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerPortal.Domain.Entities
{
	public class ResponseModel
	{

		public ResponseModel(ResponseCode responseCode, string responseMessage, object dataSet)
		{
			ResponseCode = responseCode;
			ResponseMessage = responseMessage;
			DateSet = dataSet;
		}
		public ResponseCode ResponseCode { get; set; }
		public string ResponseMessage { get; set; }
		public object DateSet { get; set; }
	}
}
