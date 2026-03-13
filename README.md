# 🏆 Gestión de Competiciones – Proyecto de Práctica

Aplicación Web desarrollada con **ASP.NET Core MVC** y **Entity Framework Core** utilizando el enfoque **Code‑First**.  
Permite gestionar **Competiciones**, **Equipos**, **Jugadores** y **Partidos**.

---

## 🚀 Tecnologías
- ASP.NET Core MVC  
- Entity Framework Core (Code‑First)  
- SQL Server / SQLite  
- Bootstrap  
- C#

---

## 📌 Funcionalidades
### Competiciones
- Crear, editar y eliminar competiciones
- Asignar equipos participantes

### Equipos
- CRUD de equipos
- Asociación con jugadores

### Jugadores
- CRUD de jugadores
- Datos básicos (nombre, posición, dorsal, etc.)

### Partidos
- Crear partidos entre equipos
- Registrar fecha y resultado

---

## 🧱 Arquitectura
- **Models**: Entidades EF Core  
- **Controllers**: Lógica MVC  
- **Views**: Razor Views  
- **Data**: DbContext y migraciones  

---

## ▶️ Ejecución
```bash
dotnet restore
dotnet ef database update
dotnet run
