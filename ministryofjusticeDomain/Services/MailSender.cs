using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Enum;
using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Services
{
    /// <summary>
    /// SendGrid email service that delivers mails to target receiver
    /// </summary>
    public class MailSender : IMailSender
    {
        /// <summary>
        /// Message sent to new members email when an account is created by the admin
        /// </summary>
        /// <param name="receiverEmail"></param>
        /// <param name="receiverName"></param>
        /// <returns></returns>
        public async Task AccountCreatedAsync(string receiverEmail, string receiverName)
        {
            var subject = "Verify your account";
            var message = "A new account has been created for you";
            await SendMail(receiverEmail, receiverName, message, subject);
        }

        /// <summary>
        /// Message sent to either department head or lawyer when a case is assigned to them
        /// </summary>
        /// <param name="receiverEmail"></param>
        /// <param name="receiverName"></param>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        public async Task NotifyAsync(string receiverEmail, string receiverName, AssignedTo assignedTo)
        {
            string subject = "Case Notification";
            string message;
            switch (assignedTo)
            {
                case AssignedTo.Department:
                    message = "A new case has been assigned to your department.";
                    break;
                case AssignedTo.Lawyer:
                    message = "A new case has been assigned to you.";
                    break;
                default:
                    message = "You have a notification";
                    break;
            }

            await SendMail(receiverEmail, receiverName, message, subject);
        }

        /// <summary>
        /// Email notifies the client about the current status of their case.
        /// </summary>
        /// <param name="receiverEmail"></param>
        /// <param name="receiverName"></param>
        /// <param name="caseId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task SendCaseStatus(string receiverEmail, string receiverName, Case currentCase)
        {
            var subject = $"Case Status Update: {currentCase.CaseID}";
            string message = "";
            switch (currentCase.StatusID)
            {
                case Status.Accepted:
                    message = $"Your case, {currentCase.CaseID} has been accepted and is currently been processed.";
                    break;
                case Status.Pending:
                    message =
                        $"Your case, {currentCase.CaseID} has been submitted successful. You will notified about the courses of account on your case";
                    break; 
                case Status.Processing:
                    message =
                        $"Your case, {currentCase.CaseID} is been processed by a laywer.";
                    break;
                case Status.Rejected:
                    message = $"Your case, {currentCase.CaseID} has been rejected.";
                    break;
            }

            await SendMail(receiverEmail, receiverName, message, subject);
        }

        private async Task SendMail(string email, string name, string message, string subject)
        {
            try
            {
                var mailService = new SendGridService();
                await mailService.SendEmailAsync(email, name, message, subject);
            }
            catch (Exception e)
            {
                throw new Exception($"Sending mail failed due to {e}");
            }
        }
    }
}
