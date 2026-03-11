# ============================================================
# Ejecutar desde: aspnet-core\src\
# Esto crea GlobalUsings.cs en los 3 proyectos afectados
# ============================================================

# --- 1. Domain.Shared ---
@"
global using System;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
"@ | Out-File -Encoding utf8 "KioskPos.Analytics.Domain.Shared\GlobalUsings.cs"

Write-Host "OK - Domain.Shared\GlobalUsings.cs" -ForegroundColor Green

# --- 2. Domain ---
@"
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
"@ | Out-File -Encoding utf8 "KioskPos.Analytics.Domain\GlobalUsings.cs"

Write-Host "OK - Domain\GlobalUsings.cs" -ForegroundColor Green

# --- 3. Infrastructure ---
@"
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
"@ | Out-File -Encoding utf8 "KioskPos.Analytics.Infrastructure\GlobalUsings.cs"

Write-Host "OK - Infrastructure\GlobalUsings.cs" -ForegroundColor Green

# --- 4. EntityFrameworkCore (por si acaso) ---
@"
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
"@ | Out-File -Encoding utf8 "KioskPos.Analytics.EntityFrameworkCore\GlobalUsings.cs"

Write-Host "OK - EntityFrameworkCore\GlobalUsings.cs" -ForegroundColor Green

Write-Host ""
Write-Host "Listo. Ahora recompila con Ctrl+Shift+B en Visual Studio." -ForegroundColor Cyan
