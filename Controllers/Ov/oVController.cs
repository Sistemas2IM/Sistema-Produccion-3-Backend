using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema_Produccion_3_Backend.Controllers.Ov
{
    [Route("api/[controller]")]
    [ApiController]
    public class oVController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public oVController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/oV
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<OVDto>>> GetoV()
        {
            var oV = await _context.oV
                .Include(u => u.articuloOv)
                .ToListAsync();

            var oVDto = _mapper.Map<List<OVDto>>(oV);

            return Ok(oVDto);
        }

        // GET: api/oV/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<OVDto>> GetoV(int id)
        {
            var oV = await _context.oV
                .Include(u => u.articuloOv)
                .FirstOrDefaultAsync(u => u.oV1 == id);
            var oVDto = _mapper.Map<OVDto>(oV);

            if (oVDto == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            return Ok(oVDto);
        }

        [HttpGet("get/checklist/{id}")]
        public ActionResult GetChecklistOvSap(int id)
        {
            SAPbobsCOM.Recordset? oRecordSet = null;

            try
            {
                // 1. Conectar a SAP
                HANAConnection.sapConn();

                if (HANAConnection.RetVal != 0)
                {
                    string errMsg = HANAConnection.OCompany.GetLastErrorDescription();
                    return StatusCode(500, $"Error al conectar a SAP: {errMsg}");
                }

                // 2. Preparar el Recordset y la Consulta
                oRecordSet = (SAPbobsCOM.Recordset)HANAConnection.OCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                // Usamos la base de datos y formato que espera HANA
                string queryHana = $@"
            SELECT 
                T0.""U_Demasia"" AS ""Acepta excedente"",
                T0.""U_OcCliente"" AS ""Orden de compra adjunta"",
                T0.""U_TipoOrden"" AS ""Tipo de arte"",
                T1.""U_AprobacionClienteMaquina"" AS ""Aprobacion de cliente en maquina"",
                T1.""U_GuiaColor"" AS ""Guia de color"",
                T1.""U_FormaEmpaque"" AS ""Forma de empaque"",
                T1.""U_Entrega"" AS ""Entregas""
            FROM ""RESPALDO_1"".""ORDR"" T0
            INNER JOIN ""RESPALDO_1"".""RDR1"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
            WHERE T0.""DocNum"" = '{id}'";

                oRecordSet.DoQuery(queryHana);

                // 3. Verificar si SAP devolvió resultados
                if (oRecordSet.EoF)
                {
                    return NotFound(new { mensaje = $"No se encontró la OV {id} en SAP." });
                }

                // 4. Extraer los valores y limpiar espacios en blanco o nulos
                string aceptaExcedente = oRecordSet.Fields.Item("Acepta excedente").Value?.ToString()?.Trim() ?? "";
                string ordenCompra = oRecordSet.Fields.Item("Orden de compra adjunta").Value?.ToString()?.Trim() ?? "";
                string tipoArte = oRecordSet.Fields.Item("Tipo de arte").Value?.ToString()?.Trim() ?? "";
                string aprobacionClienteMaquina = oRecordSet.Fields.Item("Aprobacion de cliente en maquina").Value?.ToString()?.Trim() ?? "";
                string guiaColor = oRecordSet.Fields.Item("Guia de color").Value?.ToString()?.Trim() ?? "";
                string formaEmpaque = oRecordSet.Fields.Item("Forma de empaque").Value?.ToString()?.Trim() ?? "";
                string entregas = oRecordSet.Fields.Item("Entregas").Value?.ToString()?.Trim() ?? "";

                // 5. Construir la respuesta evaluando si están Completos o Incompletos
                var checklistResponse = new
                {
                    OV = id,
                    aceptaExcedente = new
                    {
                        valor = aceptaExcedente,
                        estado = string.IsNullOrEmpty(aceptaExcedente) ? "Incompleto" : "Completado"
                    },
                    ordenCompraAdjunta = new
                    {
                        valor = ordenCompra,
                        estado = string.IsNullOrEmpty(ordenCompra) ? "Incompleto" : "Completado"
                    },
                    tipoArte = new
                    {
                        valor = tipoArte,
                        estado = string.IsNullOrEmpty(tipoArte) ? "Incompleto" : "Completado"
                    },
                    aprobacionClienteMaquina = new
                    {
                        valor = aprobacionClienteMaquina,
                        estado = string.IsNullOrEmpty(aprobacionClienteMaquina) ? "Incompleto" : "Completado"
                    },
                    guiaColor = new
                    {
                        valor = guiaColor,
                        estado = string.IsNullOrEmpty(guiaColor) ? "Incompleto" : "Completado"
                    },
                    formaEmpaque = new
                    {
                        valor = formaEmpaque,
                        estado = string.IsNullOrEmpty(formaEmpaque) ? "Incompleto" : "Completado"
                    },
                    entregas = new
                    {
                        valor = entregas,
                        estado = string.IsNullOrEmpty(entregas) ? "Incompleto" : "Completado"
                    }
                };

                return Ok(checklistResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al consultar SAP: {ex.Message}");
            }
            finally
            {
                // 6. Liberación obligatoria de memoria COM para que SAP no se congele
                if (oRecordSet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                    oRecordSet = null;
                }

                if (HANAConnection.OCompany != null && HANAConnection.OCompany.Connected)
                {
                    HANAConnection.OCompany.Disconnect();
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        // PUT: api/oV/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutoV(int id, oV oV)
        {
            if (id != oV.idOv)
            {
                return BadRequest();
            }

            _context.Entry(oV).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!oVExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/oV
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<oV>> PostoV(AddOVDto addOv)
        {
            var oV = _mapper.Map<oV>(addOv);
            _context.oV.Add(oV);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetoV", new { id = oV.oV1 }, oV);
        }

        private bool oVExists(int id)
        {
            return _context.oV.Any(e => e.oV1 == id);
        }
    }
}
