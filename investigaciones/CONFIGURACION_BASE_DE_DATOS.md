# SQL Server 2022 - adventure_works (AdventureWorks2022)

## Datos de Conexión
| Dato | Valor |
|------|-------|
| **Servidor** | `localhost,1433` |
| **Usuario** | `sa` |
| **Contraseña** | `Hela031105!` |
| **Base de Datos** | `adventure_works` |
| **Tablas** | 91 |
| **Esquemas** | 9 |
| **Contenedor** | `sqlserver` |
| **Estado** | ✅ Con datos reales (31,465 órdenes, 290 empleados, 504 productos) |

**String conexión:**
```
Server=localhost,1433;Database=adventure_works;User Id=sa;Password=Hela031105!;Encrypt=false;TrustServerCertificate=true;
```

---

## Arquitectura
```
Tu Mac → Colima → Lima VM → Docker → Contenedor (SQL Server)
                     ↓
                Kernel Linux
                Namespaces + Cgroups
                Proceso sqlservr
```

---

## ¿Cómo Funciona?
1. **Colima:** Gestor de VM ligero (Mac → Lima VM)
2. **Lima VM:** Linux mínimo con Docker daemon
3. **Namespaces:** Aislamiento de procesos (PID), red (Network), archivos (Mount)
4. **Cgroups:** Limitan recursos (CPU, RAM, I/O)
5. **Contenedor:** Proceso sqlservr aislado en `172.17.0.2:1433`
6. **Port forwarding:** `localhost:1433` → contenedor:1433

---

## Herramientas
| Herramienta | Uso | Recomendación |
|---|---|---|
| **MSSQL (VS Code)** | Directa a SQL Server | ✅ USA ESTO |
| **MCP (sql-server-copilot)** | Intermediario (más lento) | ❌ No necesario |
| **DBeaver** | GUI visual | ✅ Alternativa |

---

## Colima vs Docker Desktop
| Aspecto | Colima | Docker Desktop |
|---|---|---|
| **RAM** | ~500MB-1GB | ~2-3GB |
| **Apple Silicon** | Nativo ✅ | Rosetta 2 |
| **Precio** | Gratis | Con limitaciones |
| **Tu caso** | ✅ USAS ESTO | No compatible |

---

## Comandos

**Colima:**
```bash
colima start    # Inicia VM
colima stop     # Detiene VM
colima status   # Ver estado
```

**Docker:**
```bash
docker ps                      # Ver contenedores
docker logs sqlserver          # Ver logs
docker exec -it sqlserver /bin/bash  # Acceder
```

**SQL Query:**
```bash
docker exec -i sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U SA -P 'Hela031105!' \
  -d adventure_works \
  -Q "SELECT COUNT(*) FROM sys.tables"
```

---

## Usar en VS Code

1. **Conectar:** Cmd+Shift+P → "MSSQL: Create Connection"
2. **Datos:** 
   - Server: `localhost,1433`
   - User: `sa`
   - Password: `Hela031105!`
   - Database: `adventure_works`
3. **Query:** Crea `.sql` → Ctrl+Shift+E → Ver resultados

---

**Setup:** Colima + Docker + SQL Server 2022 (RTM-CU24-GDR) - v16.0.4250.1  
**OS:** Linux (Ubuntu 22.04.5 LTS) - Developer Edition  
**Archivo BD:** `/var/opt/mssql/data/adventure_works.mdf`  
**Fuente:** AdventureWorks2022.bak restaurada con datos reales completos  
**Datos:** 31,465 órdenes de venta, 121,317 detalles, 290 empleados, 19,820 clientes
