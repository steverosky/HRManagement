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


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            _logger.LogInformation("Login method Starting.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ResponseType type = ResponseType.Success;
                var model = await _db.Login(user);

                return Ok(ResponseHandler.GetAppResponse(type, model));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        //[Authorize]
        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<ActionResult<List<EmployeeModel>>> GetEmployees()
        {
            
            ResponseType type = ResponseType.Success;
            _logger.LogInformation("Get all employees method Starting.");
            try
            {
                var data = await _db.GetAllEmp();

                if (data is not null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        //[Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<List<Users>>> GetUsers()
        {

            ResponseType type = ResponseType.Success;
            _logger.LogInformation("Get all users method Starting.");
            try
            {
                var data = await _db.GetAllUsers();

                if (data is not null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        //[Authorize]
        [HttpGet]
        [Route("GetEmployeeByEmail")]
        public async Task<ActionResult<EmployeeModel>> GetEmployeeByEmail(string email)
        {
            ResponseType type = ResponseType.Success;
            _logger.LogInformation("Get employee by email method Starting.");
            try
            {
                EmployeeModel data = await _db.GetEmployee(email);
                if (data == null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        [HttpGet]
        [Route("GetUserByEmail")]
        public async Task<ActionResult<Users>> GetUserByEmail(string email)
        {
            ResponseType type = ResponseType.Success;
            _logger.LogInformation("Get user by email method Starting.");
            try
            {
                Users data = await _db.GetUser(email);
                if (data == null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandler.GetAppResponse(type, data));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        //[Authorize]
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult> AddNewEmployee([FromBody] AddEmployeeModel user)
        {
            _logger.LogInformation("Add employee method Starting.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ResponseType type = ResponseType.Success;
                await _db.AddEmployee(user);

                _logger.LogWarning($"User {user.Email} created successfully");
                return Ok(ResponseHandler.GetAppResponse(type, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        //[Authorize]
        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult> AddNewUser([FromBody] UserLogin user)
        {
            _logger.LogInformation("Add user method Starting.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ResponseType type = ResponseType.Success;
                await _db.AddUser(user);

                _logger.LogWarning($"User {user.Email} created successfully");
                return Ok(ResponseHandler.GetAppResponse(type, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        //[Authorize]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<ActionResult<RoleModel>> CreateRole(AddRoleModel role)
        {
            ResponseType type = ResponseType.Success;
            _logger.LogInformation("Create role method Starting.");
            try
            {
                var data = await _db.CreateRole(role);

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

        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        //[HttpPost]
        //[Route("AddUser")]
        //public IActionResult Post([FromBody] EmployeeModel model)
        //{
        //    // Validate the model and the default role
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        ResponseType type = ResponseType.Success;
        //        _db.AddUser(model);

        //        return Ok(ResponseHandler.GetAppResponse(type, model));

        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        //    }
        //}
    }
}
