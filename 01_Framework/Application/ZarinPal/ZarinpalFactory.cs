﻿using Microsoft.Extensions.Configuration;
using RestSharp;

namespace _0_Framework.Application.ZarinPal
{
    public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly string _baseUrl;

        private readonly IConfiguration _configuration;

        public string Prefix { get; set; }
        private string MerchantId { get; }

        public ZarinPalFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            Prefix = _configuration.GetSection("payment")["method"];
            MerchantId = _configuration.GetSection("payment")["merchant"];
            _baseUrl = $"https://{Prefix}.zarinpal.com/pg/rest/WebGate/";
        }

        public PaymentResponse CreatePaymentRequest(string amount, string mobile, string email, string description,
             long orderId)
        {
            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);
            var siteUrl = _configuration.GetSection("payment")["siteUrl"];

            var client = new RestClient(_baseUrl);
            var request = new RestRequest("PaymentRequest.json", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = new PaymentRequest
            {
                Mobile = mobile,
                CallbackURL = $"{siteUrl}/CheckOut?handler=CallBack&oId={orderId}",
                Description = description,
                Email = email,
                Amount = finalAmount,
                MerchantID = MerchantId
            };
            request.AddJsonBody(body);
            var response = client.Execute(request);

            var CallbackURL = $"{siteUrl}/CheckOut?handler=CallBack&oId={orderId}";

            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
            return res;
        }

        public VerificationResponse CreateVerificationRequest(string authority, string amount)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("PaymentVerification.json", Method.Post);
            request.AddHeader("Content-Type", "application/json");

            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);

            request.AddJsonBody(new VerificationRequest
            {
                Amount = finalAmount,
                MerchantID = MerchantId,
                Authority = authority
            });
            var response = client.Execute(request);
            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<VerificationResponse>(response.Content);
            return res;
        }
    }
}