using Moq;
using NUnit.Framework;
using Security.Models;
using Security.Services;

namespace Security.Tests.Services;

[TestFixture]
public class AuthServiceTests {

    private readonly IAuthService _authService;
    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<SecurityContext> _context;

    public AuthServiceTests() {
        
        _configuration = new Mock<IConfiguration>();
        // _context = new Mock<SecurityContext>();
        // _authService = new AuthService(_configuration.Object, _context.Object);
    }

    [Test]
    public void Test() {
        Assert.IsTrue(true);
    }



}