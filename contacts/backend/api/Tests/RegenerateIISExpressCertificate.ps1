# https://stackoverflow.com/questions/64880715/https-error-in-asp-net-core-app-running-on-iisexpress-pr-connect-reset-error
Start-Transcript -Path "$($MyInvocation.MyCommand.Path).log"
try {
    Write-Host "Creating cert resources"
    $ekuOidCollection = [System.Security.Cryptography.OidCollection]::new();
    $ekuOidCollection.Add([System.Security.Cryptography.Oid]::new("1.3.6.1.5.5.7.3.1","Server Authentication")) | Out-Null
    $sanBuilder = [System.Security.Cryptography.X509Certificates.SubjectAlternativeNameBuilder]::new();
    $sanBuilder.AddDnsName("localhost") | Out-Null

    Write-Host "Creating cert extensions"
    $certificateExtensions = @(
        # Subject Alternative Name
        $sanBuilder.Build($true),
        # ASP.NET Core OID
        [System.Security.Cryptography.X509Certificates.X509Extension]::new(
            "1.3.6.1.4.1.311.84.1.1",
            [System.Text.Encoding]::ASCII.GetBytes("IIS Express Development Certificate"),
            $false),
            # KeyUsage
            [System.Security.Cryptography.X509Certificates.X509KeyUsageExtension]::new(
                [System.Security.Cryptography.X509Certificates.X509KeyUsageFlags]::KeyEncipherment,
                $true),
                # Enhanced key usage
        [System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension]::new(
            $ekuOidCollection,
            $true),
            # Basic constraints
            [System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension]::new($false,$false,0,$true)
        )
    Write-Host "Creating cert parameters"
    $parameters = @{
        Subject = "localhost";
        KeyAlgorithm = "RSA";
        KeyLength = 2048;
        CertStoreLocation = "Cert:\LocalMachine\My";
        KeyExportPolicy = "Exportable";
        NotBefore = Get-Date;
        NotAfter = (Get-Date).AddYears(1);
        HashAlgorithm = "SHA256";
        Extension = $certificateExtensions;
        SuppressOid = @("2.5.29.14");
        FriendlyName = "IIS Express Development Certificate"
    }
    Write-Host "Creating cert"
    $cert = New-SelfSignedCertificate @parameters

    $rootStore = New-Object System.Security.Cryptography.X509Certificates.X509Store -ArgumentList Root, LocalMachine
    $rootStore.Open("MaxAllowed")
    $rootStore.Add($cert)
    $rootStore.Close()

    Write-Host "Creating port bindings"
    # Add an Http.Sys binding for port 44300-44399
    $command = 'netsh'
    for ($i=44300; $i -le 44399; $i++) {
        $optionsDelete = @('http', 'delete', 'sslcert', "ipport=0.0.0.0:$i")
        $optionsAdd = @('http', 'add', 'sslcert', "ipport=0.0.0.0:$i", "certhash=$($cert.Thumbprint)", 'appid={214124cd-d05b-4309-9af9-9caa44b2b74a}')
        Write-Host "Running $command $optionsDelete"
        & $command $optionsDelete
        Write-Host "Running $command $optionsAdd"
        & $command $optionsAdd
    }
}
catch {
    Write-Error $_.Exception.Message
}
finally {
    Stop-Transcript
}