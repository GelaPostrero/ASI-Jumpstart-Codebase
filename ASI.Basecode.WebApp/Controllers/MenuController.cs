using ASI.Basecode.Resources.Constants;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Utils.Response;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.HelperFunctions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace ASI.Basecode.WebApp.Controllers
{
    [ApiController]
    [Route(AppConstants.Controllers.Menu)]
    public class MenuController : ControllerBase
    {
    private readonly SessionManager _sessionManager;
    private readonly SignInManager _signInManager;
    private readonly TokenValidationParametersFactory _tokenValidationParametersFactory;
    private readonly TokenProviderOptionsFactory _tokenProviderOptionsFactory;
    private readonly IConfiguration _appConfiguration;
    private readonly IUserService _userService;
    private readonly IMenuService menuService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<MenuController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuController"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="tokenValidationParametersFactory">The token validation parameters factory.</param>
        /// <param name="tokenProviderOptionsFactory">The token provider options factory.</param>
        public MenuController(
            SignInManager signInManager,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IMenuService menuService,
            IConfiguration configuration,
            IMapper mapper,
            IUserService userService,
            TokenValidationParametersFactory tokenValidationParametersFactory,
            TokenProviderOptionsFactory tokenProviderOptionsFactory,
            ILogger<MenuController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionManager = new SessionManager(httpContextAccessor.HttpContext.Session);
            _signInManager = signInManager;
            _tokenProviderOptionsFactory = tokenProviderOptionsFactory;
            _tokenValidationParametersFactory = tokenValidationParametersFactory;
            _appConfiguration = configuration;
            _userService = userService;
            this.menuService = menuService;
            _logger = logger;
        }

        /// <summary>
        /// Get All Menu Method
        /// </summary>
        /// <returns>List of all Menus.</returns>
        [HttpGet]
        //[AllowAnonymous]
        public IActionResult GetAllMenus()
        {
            var returnList = menuService.GetAllMenu();
            return StatusCode((int)HttpStatusCode.OK, new ResponseModel(returnList, string.Empty));
        }

        /// <summary>
        /// Add Menu Method
        /// </summary>
        /// <param name="menuRequestViewModel"></param>
        /// <returns>Response Model for the Add/Insertion operation.</returns>
        [HttpPost(Name = nameof(AddMenu))]
        public IActionResult AddMenu([FromBody] MenuRequestViewModel menuRequestViewModel)
        {
            try
            {
                var returnCode = menuService.AddMenu(menuRequestViewModel, 1); //TODO: Get userId from session
                var message = HelperFunctions.CommonHelper.GetAddResultMessage(returnCode);
                return StatusCode(message.StatusCode, message.Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseModel(string.Empty, Resources.Messages.Common.ItemNotAdded));
            }
        }

        /// <summary>
        /// Update Menu Method
        /// </summary>
        /// <param name="menuRequestViewModel"></param>
        /// <returns>Response Model for the Update operation.</returns>
        [HttpPut(Name = nameof(UpdateMenu))]
        public IActionResult UpdateMenu([FromBody] MenuRequestViewModel menuRequestViewModel)
        {
            try
            {
                var returnCode = menuService.UpdateMenu(menuRequestViewModel, 1); //TODO: Get userId from session
                var message = HelperFunctions.CommonHelper.GetUpdateResultMessage(returnCode);
                return StatusCode(message.StatusCode, message.Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseModel(string.Empty, Resources.Messages.Common.UpdateFailed));
            }
        }

        [HttpDelete(Name = nameof(BatchDeleteMenu))]
        public IActionResult BatchDeleteMenu([FromBody] int[] ids)
        {
            try
            {
                var returnValues = menuService.BatchDeleteDocument(ids, 1); //TODO: Get userId from session
                var messages = CommonHelper.GetResultCount(returnValues, AppConstants.Delete);
                var sb = new StringBuilder().AppendJoin(AppConstants.Space, messages).ToString().Trim();
                var returnCode = returnValues.Contains(AppConstants.CrudStatusCodes.DoesNotExist) ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;
                return StatusCode(returnCode, new ResponseModel(null, sb));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseModel(string.Empty, Resources.Messages.Common.ListNotDeleted));
            }
        }
    }
}
