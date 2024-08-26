using System;
using System.Collections.Generic;
using System.Linq;
using _01_Framework.Application.Sms;
using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace _0_Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        //private readonly IConfiguration _configuration;

        //public SmsService(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //public void Send(string number, string message)
        //{
        //    var token = GetToken();
        //    var lines = new SmsLine().GetSmsLines(token);
        //    if (lines == null) return;

        //    var line = lines.SMSLines.Last().LineNumber.ToString();
        //    var data = new MessageSendObject
        //    {
        //        Messages = new List<string>
        //            {message}.ToArray(),
        //        MobileNumbers = new List<string> { number }.ToArray(),
        //        LineNumber = line,
        //        SendDateTime = DateTime.Now,
        //        CanContinueInCaseOfError = true
        //    };
        //    var messageSendResponseObject =
        //        new MessageSend().Send(token, data);

        //    if (messageSendResponseObject.IsSuccessful) return;

        //    line = lines.SMSLines.First().LineNumber.ToString();
        //    data.LineNumber = line;
        //    new MessageSend().Send(token, data);
        //}

        //private string GetToken()
        //{
        //    var smsSecrets = _configuration.GetSection("SmsSecrets");
        //    var tokenService = new Token();
        //    return tokenService.GetToken(smsSecrets["ApiKey"], smsSecrets["SecretKey"]);
        //}
        private readonly IConfiguration _configuration;
        private readonly VerficationCodeService _verficationCodeService;

        public SmsService(IConfiguration configuration, VerficationCodeService verficationCodeService)
        {
            _configuration = configuration;
            _verficationCodeService = verficationCodeService;
        }

        //public async Task<bool> SendVerificationCodeAsync(string moblie)
        //{
        //    var apiKey = _configuration.GetSection("SmsSetting")["ApiKey"];

        //    try
        //    {
        //        SmsIr smsIr = new SmsIr(apiKey);

        //        int templateId = 990782; //990782 custom template Id

        //        int code = Random.Shared.Next(111111, 999999);

        //        _verficationCodeService.SetVerficationCode(code);

        //        VerifySendParameter[] verifySendParameters = {
        //        new VerifySendParameter("CODE", code.ToString()),
        //    };

        //        // انجام ارسال وریفای
        //        var response = await smsIr.VerifySendAsync(moblie, templateId, verifySendParameters);

        //        // ارسال شما در اینجا با موفقیت انجام شده‌است.

        //        // گرفتن بخش دیتا خروجی
        //        VerifySendResult verifySendResult = response.Data;

        //        // شناسه پیامک ارسال شده
        //        int messageId = verifySendResult.MessageId;

        //        // هزینه ارسال
        //        decimal cost = verifySendResult.Cost;

        //        string resultDescription = "Your message was sent successfully." +
        //            $"\n - Message Id: {messageId} " +
        //            $"\n - Cost: {cost}";


        //        return true;
        //        //await Console.Out.WriteLineAsync(resultDescription);
        //    }
        //    catch (Exception ex) // درخواست ناموفق
        //    {
        //        // جدول توضیحات کد وضعیت
        //        // https://app.sms.ir/developer/help/statusCode

        //        string errorName = ex.GetType().Name;
        //        string errorNameDescription = errorName switch
        //        {
        //            "UnauthorizedException" => "The provided token is not valid or access is denied.",
        //            "LogicalException" => "Please check and correct the request parameters.",
        //            "TooManyRequestException" => "The request count has exceeded the allowed limit.",
        //            "UnexpectedException" or "InvalidOperationException" => "An unexpected error occurred on the remote server.",
        //            _ => "Unable to send the request due to an unspecified error.",
        //        };

        //        var errorDescription = "There is a problem with the request." +
        //            $"\n - Error: {errorName} - {errorNameDescription} - {ex.Message}";


        //        return false;
        //        //await Console.Out.WriteLineAsync(errorDescription);
        //    }
        //}

        public async Task<bool> SendVerificationCodeAsync(string mobile)
        {
            int code = Random.Shared.Next(111111, 999999);

            Console.WriteLine(code);

            VerificationModel verificationModel = new(mobile, code);

            _verficationCodeService.AddVerification(verificationModel);

            return true;
        }
    }
}