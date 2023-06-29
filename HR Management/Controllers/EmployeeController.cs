namespace HR_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IDBServices _db;
        private readonly DataContext _context;
        public EmployeeController(IConfiguration config, ILogger<EmployeeController> logger, DataContext context, IDBServices db)
        {
            _configuration = config;
            _db = db;
            _logger = logger;
            _logger.LogInformation("User controller called ");
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateRole")]
        public ActionResult<RoleModel> CreateRole(AddRoleModel role)
        {
            ResponseType type = ResponseType.Success;
            _logger.LogInformation("Create role method Starting.");
            try
            {
                var data = _db.CreateRole(role);

                if (data is null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }
    }
}
