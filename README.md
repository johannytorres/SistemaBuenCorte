# SistemaBuenCorte
Sistema de facturación e inventario para la carnicería El Buen Corte. Proyecto de Programación III.

## Resumen
Aplicación web multicapa (API .NET + React) para gestionar productos, inventario y ventas. Usa SQL Server LocalDB y Entity Framework Core. Este README contiene pasos reproducibles para ejecutar la solución localmente.

## Tecnologías
- C# / .NET 9 (net9.0)
- ASP.NET Core
- Entity Framework Core 9
- SQL Server LocalDB
- React (Create React App)
- Node.js

## Requisitos
- .NET 9 SDK
- SQL Server LocalDB (localdb)\MSSQLLocalDB
- Node.js 18+
- dotnet-ef tool (opcional local): `dotnet tool install --global dotnet-ef`

## Estructura
```
SistemaBuenCorte/
├── SistemaBuenCorte.sln
├── src/
│   ├── SistemaBuenCorte.Web/    # API + proyecto web
│   ├── SistemaBuenCorte.BLL/    # Lógica de negocio
│   └── SistemaBuenCorte.DAL/    # DbContext, entidades, migraciones
```

## Pasos para ejecutar (desde la raíz del repo)
1) Asegurar `develop` actualizado:
```bash
git checkout develop
git pull origin develop
```

2) Iniciar LocalDB (si no está):
```powershell
sqllocaldb start MSSQLLocalDB
```

3) Aplicar migraciones y crear la base `ElBuenCorte`:
(Detener cualquier `dotnet run` antes de ejecutar)
```bash
dotnet restore
dotnet ef database update --project src\SistemaBuenCorte.DAL --startup-project src\SistemaBuenCorte.Web
```

4) Ejecutar backend (Terminal A):
```bash
dotnet run --project src\SistemaBuenCorte.Web
```

5) Ejecutar frontend (Terminal B):
```bash
cd src\SistemaBuenCorte.Web
npm install
npm start
```
O, para el sub-app `login-react-ts`:
```bash
cd src\SistemaBuenCorte.Web\login-react-ts
npm install
npm start
```

6) Credenciales de prueba (datos semilla):
- admin / Admin123!
- cajero / Cajero123!

Las contraseñas se almacenan como hashes BCrypt en la BD.

## Cómo actualizar semillas (si hace falta)
- Generar hashes BCrypt localmente y reemplazar `ContrasenaHash` en `AppDbContext.OnModelCreating`.
- Crear migración desde la raíz:
```bash
dotnet ef migrations add UpdateSeedPasswords --project src\SistemaBuenCorte.DAL --startup-project src\SistemaBuenCorte.Web
dotnet ef database update --project src\SistemaBuenCorte.DAL --startup-project src\SistemaBuenCorte.Web
```

## Recomendaciones para la profesora (hacerlo más sencillo)
- Incluir `setup.ps1` o `setup.bat` que: arranque LocalDB, aplique migraciones y deje backend escuchando.
- Proveer un script `seed.sql` o migración incluida para no requerir pasos manuales.
- Añadir instrucciones en el PR sobre cerrar servidores antes de migraciones y habilitar `git config --global core.longpaths true` en Windows si hay errores de ruta larga.

## Buenas prácticas
- No subir `node_modules` (ya en .gitignore).
- Usar rama feature → PR → merge a `develop`.

## Autores
- Dioris Arias — @usuario_github
- Gabriel Cuevas — @usuario_github
- Johanny Torres — @usuario_github
- Josue David Guerrero — @usuario_github
- Mayerlin Alcántara — @usuario_github

---
Programación III · ITLA · 2026