using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Bingo.Api.Shared;
using Nuke.Common;
using Nuke.Common.IO;

var repositoryRootFolder = AbsolutePath.Create(FileSystemHelper.FindDirectoryContaining("bingo-frontend"));
var path = repositoryRootFolder / "bingo-frontend" / "tls";
var caPath = path / "ca.pem";
var certPath = path / "cert.crt";
var keyPath = path / "cert.key";

if (IsAdministrator())
{
    InstallCert(caPath);
    return;
}

// Generate CA
var rootKey = RSA.Create(4096);
var caCertReq = new CertificateRequest(
    "CN=Karibou Cert Authority",
    rootKey,
    HashAlgorithmName.SHA256,
    RSASignaturePadding.Pkcs1
);
caCertReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
caCertReq.CertificateExtensions.Add(new X509KeyUsageExtension(
    X509KeyUsageFlags.KeyCertSign |
    X509KeyUsageFlags.DigitalSignature |
    X509KeyUsageFlags.CrlSign
    , false));
caCertReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(caCertReq.PublicKey, false));

var subjectKeyIdentifier = SHA1.HashData(caCertReq.PublicKey.EncodedKeyValue.RawData);
caCertReq.CertificateExtensions.Add(X509AuthorityKeyIdentifierExtension.CreateFromSubjectKeyIdentifier(subjectKeyIdentifier));

var rootCert = caCertReq.CreateSelfSigned(
    DateTimeOffset.Now.AddDays(-2),
    DateTimeOffset.Now.AddYears(20)
);

// Generate Cert
var certKey = RSA.Create(4096);
var certReq = new CertificateRequest(
    "CN=Karibou Dev Wildcard",
    certKey,
    HashAlgorithmName.SHA256,
    RSASignaturePadding.Pkcs1
);
certReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
caCertReq.CertificateExtensions.Add(new X509KeyUsageExtension(
    X509KeyUsageFlags.DigitalSignature |
    X509KeyUsageFlags.NonRepudiation |
    X509KeyUsageFlags.KeyEncipherment
    , false));

var subjectAlternativeNameBuilder = new SubjectAlternativeNameBuilder();
subjectAlternativeNameBuilder.AddDnsName("localhost");
certReq.CertificateExtensions.Add(subjectAlternativeNameBuilder.Build());

// <summary>
// Indicates that a certificate can be used as an SSL server certificate
// https://oidref.com/1.3.6.1.5.5.7.3.1
// </summary>
var serverAuthOid = new Oid("1.3.6.1.5.5.7.3.1");
certReq.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension([serverAuthOid], true));

var expirationTime = DateTimeOffset.Now.AddYears(19);
if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    expirationTime = DateTimeOffset.Now.AddDays(800); // 825 days max: https://support.apple.com/en-ca/103769

var serial = new byte[sizeof(long)];
RandomNumberGenerator.Fill(serial);

var cert = certReq.Create(
    rootCert,
    DateTimeOffset.Now.AddDays(-1),
    expirationTime,
    serial
);

// Save certs to folder
caPath.WriteAllText(rootCert.ExportCertificatePem());
certPath.WriteAllText(cert.ExportCertificatePem());
keyPath.WriteAllText(certKey.ExportRSAPrivateKeyPem());

// Re-execute into root to install certs
if (OperatingSystem.IsWindows())
{
    var proc = new Process
    {
        StartInfo =
        {
            FileName = Environment.ProcessPath.NotNull(),
            UseShellExecute = true,
            Verb = "runas",
        },
    };
    
    proc.Start();
    await proc.WaitForExitAsync();
}

Console.WriteLine("Certificate created in : " + path.ToString());
return;


static bool IsAdministrator()
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    return Environment.UserName == "root";
}

// Load cert into the system
static void InstallCert(AbsolutePath caPath)
{
    if (OperatingSystem.IsWindows())
    {
        var caCert = X509CertificateLoader.LoadCertificateFromFile(caPath);
        using var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        store.Open(OpenFlags.ReadWrite);
        var oldCertificates = store.Certificates.Find(X509FindType.FindBySubjectName, caCert.SubjectName.Name, false);
        foreach (var oldCert in oldCertificates)
            store.Remove(oldCert);
        store.Add(X509CertificateLoader.LoadCertificate(caCert.RawData));
    }
}