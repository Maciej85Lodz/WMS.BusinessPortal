using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Models;
using WMS.Models.Invent;

namespace WMS.Services
{
    public interface INetcoreService
    {
        Task SendEmailBySendGridAsync(string apiKey, string fromEmail, string fromFullName, string subject, string message, string email);

        Task<bool> IsAccountActivatedAsync(string email, UserManager<ApplicationUser> userManager);

        Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL);

        Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env);

        Task UpdateRoles(ApplicationUser appUser, ApplicationUser currentUserLogin);

        Task CreateDefaultSuperAdmin();

        VMStock GetStockByItemTypeAndWarehouse(string ItemTypeId, string warehouseId);

        List<VMStock> GetStockPerWarehouse();

        List<TimelineViewModel> GetTimelinesByBranchId(string branchId);

        List<TimelineViewModel> GetTimelinesByItemTypeId(string ItemTypeId);

        List<TimelineViewModel> GetTimelinesByVendorId(string vendorId);

        List<TimelineViewModel> GetTimelinesByCustomerId(string customerId);

        List<TimelineViewModel> GetTimelinesByPurchaseId(string purchaseId);

        List<TimelineViewModel> GetTimelinesBySalesId(string salesId);

        List<TimelineViewModel> GetTimelinesByTransferId(string transferId);

        Task InitDemo();
    }
}
