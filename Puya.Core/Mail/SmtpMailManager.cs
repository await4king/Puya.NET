using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Puya.Extensions;
using Puya.Logging;

namespace Puya.Mail
{
    public class SmtpMailManager : IMailManager
    {
        private IMailConfig config;
        private ILogger _logger;
        public SmtpMailManager(IMailConfig config, ILogger logger)
        {
            this.config = config;
            _logger = logger;
        }
        public IMailConfig Config
        {
            get { return config; }
            set { config = value; }
        }

        private bool send(MailMessage mail)
        {
            var result = false;

            try
            {
                SmtpClient client;

                if (config.Port > 0)
                {
                    client = new SmtpClient(config.Host, config.Port);
                }
                else
                {
                    client = new SmtpClient(config.Host);
                }

                if (config.EnableSSL)
                {
                    client.EnableSsl = config.EnableSSL;
                }

                client.UseDefaultCredentials = config.UseDefaultCredentials;

                if (!config.UseDefaultCredentials)
                    client.Credentials = new NetworkCredential(config.Username, config.Password);

                _logger.Info("Mail.Smtp sending ...", mail);

                client.Send(mail);

                result = true;
            }
            catch (Exception e)
            {
                _logger.Danger(e, mail);
            }

            return result;
        }
        private async Task<bool> sendAsync(MailMessage mail)
        {
            var result = false;

            try
            {
                SmtpClient client;

                if (config.Port > 0)
                {
                    client = new SmtpClient(config.Host, config.Port);
                }
                else
                {
                    client = new SmtpClient(config.Host);
                }

                client.EnableSsl = config.EnableSSL;
                client.UseDefaultCredentials = config.UseDefaultCredentials;

                if (!config.UseDefaultCredentials)
                    client.Credentials = new NetworkCredential(config.Username, config.Password);

                await _logger.InfoAsync("Mail.Smtp sending ...", mail);

                await client.SendMailAsync(mail);

                result = true;
            }
            catch (Exception e)
            {
                await _logger.DangerAsync(e, mail);
            }

            return result;
        }
        public bool Send(string to, string subject, string body, bool isHtml = false)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(config.DefaultMail);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            return send(mail);
        }
        public Task<bool> SendAsync(string to, string subject, string body, bool isHtml = false)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(config.DefaultMail);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            return sendAsync(mail);
        }
        public bool Send(string to, string subject, string body, bool isHtml, IEnumerable<string> cc, IEnumerable<string> bcc)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(config.DefaultMail);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            if (cc != null && cc.Count() > 0)
            {
                foreach (string address in cc)
                {
                    try
                    {
                        mail.CC.Add(new MailAddress(address));
                    }
                    catch (Exception e)
                    {
                        _logger.Warn($"cc address error {address}: {e.ToString('\n')}");
                    }
                }
            }

            if (bcc != null && bcc.Count() > 0)
            {
                foreach (string address in bcc)
                {
                    try
                    {
                        mail.Bcc.Add(new MailAddress(address));
                    }
                    catch (Exception e)
                    {
                        _logger.Warn($"bcc address error {address}: {e.ToString('\n')}");
                    }
                }
            }

            return send(mail);
        }
        public async Task<bool> SendAsync(string to, string subject, string body, bool isHtml, IEnumerable<string> cc, IEnumerable<string> bcc)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(config.DefaultMail);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            if (cc != null && cc.Count() > 0)
            {
                foreach (string address in cc)
                {
                    try
                    {
                        mail.CC.Add(new MailAddress(address));
                    }
                    catch (Exception e)
                    {
                        await _logger.WarnAsync($"cc address error {address}: {e.ToString('\n')}");
                    }
                }
            }

            if (bcc != null && bcc.Count() > 0)
            {
                foreach (string address in bcc)
                {
                    try
                    {
                        mail.Bcc.Add(new MailAddress(address));
                    }
                    catch (Exception e)
                    {
                        await _logger.WarnAsync($"bcc address error {address}: {e.ToString('\n')}");
                    }
                }
            }

            return await sendAsync(mail);
        }
    }
}