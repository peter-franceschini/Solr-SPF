using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.AuthorizationHandlers;
using Website.Helpers;
using Website.Models.ExplainViewModels;
using Website.Services;

namespace Website.Controllers
{
    public class ExplainController : Controller
    {
        private ISolrResponseStorageService SolrResponseStorageService { get; set; }
        private ISessionService SessionService { get; set; }
        private readonly IAuthorizationService AuthorizationService;

        public ExplainController(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, ISessionService sessionService, ISolrResponseStorageService solrResponseStorageService)
        {
            SolrResponseStorageService = solrResponseStorageService;
            SessionService = sessionService;
            AuthorizationService = authorizationService;
        }

        public ActionResult Index()
        {
            var model = new ExplainListingViewModel();
            model.RecordList = SolrResponseStorageService.GetPublicSolrQueryResponseRecords();

            return View(model);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [Authorize]
        public ActionResult MyExplains()
        {
            var model = new ExplainListingViewModel();
            if (SessionService.IsAuthenticated())
            {
                model.RecordList = SolrResponseStorageService.GetSolrQueryResponseRecords(SessionService.GetUserId());
            }

            return View(model);
        }

        public ActionResult Explain(string guid)
        {
            var responseRecord = SolrResponseStorageService.GetSolrQueryResponseRecord(guid);
            
            // Authorization
            var authorizationResult = AuthorizationService.AuthorizeAsync(User, responseRecord, Operations.Read).Result;

            if (!authorizationResult.Succeeded && User.Identity.IsAuthenticated)
            {
                // The user is already authenticated and they don't have access to this page, so return ForbidResult
                return new ForbidResult();
            }
            else if(!authorizationResult.Succeeded)
            {
                // The user is not authenticated, so ask them to authenticate as they may have access to this page once they do
                return new ChallengeResult();
            }

            var solrQueryResponse = SolrResponseStorageService.GetSolrQueryResponseFromStorage(guid);
            var model = new ExplainViewModel(guid, solrQueryResponse);

            return View("Explain", model);
        }

        public ActionResult Comparison(string guid, int documentOneIndex, int documentTwoIndex)
        {
            var solrQueryResponse = SolrResponseStorageService.GetSolrQueryResponseFromStorage(guid);
            var comparisonModel = new ExplainComparisonViewModel(solrQueryResponse, documentOneIndex, documentTwoIndex);

            return View("comparison", comparisonModel);
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            if (!files.Any())
            {
                // Nothing uploaded, return bad request
                return BadRequest();
            }

            // Store raw solrQueryResponse as a string
            var rawSolrQueryResponse = FormFileHelper.ConvertToString(files.First());

            // Store object in database
            var guid = SolrResponseStorageService.SaveSolrQueryResponse(rawSolrQueryResponse, SessionService.GetUserId());
            
            return RedirectToAction("Explain", new { guid = guid });
        }

        [HttpPost("UploadSolrRaw")]
        public async Task<IActionResult> UploadSolrRaw(string raw)
        {
            if (string.IsNullOrEmpty(raw))
            {
                // Bad request
                return BadRequest();
            }

            // Store raw solrQueryResponse as a string
            var rawSolrQueryResponse = raw;

            // Store object in database
            var guid = SolrResponseStorageService.SaveSolrQueryResponse(rawSolrQueryResponse, SessionService.GetUserId());

            return RedirectToAction("Explain", new { guid = guid });
        }
    }
}