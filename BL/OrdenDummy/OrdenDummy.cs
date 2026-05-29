using ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data.Odbc;

namespace BL.OrdenDummy
{
    public class OrdenDummy
    {
        //Catalogo de items
        public static ML.Result GetItems(string mode)
        {
         ML.Result result = new ML.Result();
         try
            {
                using(OdbcConnection connection = new OdbcConnection(DL.Connection.GetConnectionStringGen(mode)))
                {
                    connection.Open();
                    string query = $@"SELECT 'SRS'||TRIM(A.sku),TRIM(des_codfis) desc 
                    FROM skuDummys A,cat_codfis B 
                    WHERE B.cve_codfis = A.sku[3,10]";
                  List<ML.OrdenDummy.ItemDummy> items = new List<ML.OrdenDummy.ItemDummy>();
                    using(OdbcCommand command = new OdbcCommand(query, connection))
                    {
                        using(OdbcDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                ML.OrdenDummy.ItemDummy item = new ML.OrdenDummy.ItemDummy();
                                item.SkuOrden = reader.GetString(0);
                                item.Descripcion = reader.GetString(1);
                                items.Add(item);
                            }
                        }
                    }
                    result.Correct = true;
                    result.Object = items;
                }
            
            }
            catch(Exception ex)         {
            result.Correct = false;
            result.Message = "Error obteniendo items: " + ex.Message;
            result.Ex = ex;
         }
            return result;
        }
        //Catalogo de tiendas
        public static ML.Result GetTiendas(string mode)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(OdbcConnection connection = new OdbcConnection(DL.Connection.GetConnectionStringGen(mode)))
                {
                    connection.Open();
                    string query = @"SELECT A.cod_pto,
                                   CASE WHEN (B.facility IS NULL)
                                   THEN 'SRS'||A.cod_pto
                                   ELSE
                                   TRIM(B.facility)
                                   END AS cod_ora,
                                   TRIM(A.des_pto) desc
                                   FROM puntos A, OUTER ora_fac_go B
                                   WHERE A.cod_emp = 1
                                   AND A.cod_pto NOT IN (870)
                                   AND B.cen_pto = A.cod_pto";
                    List<ML.OrdenDummy.TiendaDummy> tiendas = new List<ML.OrdenDummy.TiendaDummy>();
                    using(OdbcCommand command = new OdbcCommand(query, connection))
                    {
                        using(OdbcDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                ML.OrdenDummy.TiendaDummy tienda = new ML.OrdenDummy.TiendaDummy();
                                tienda.CodigoOracle = reader.GetString(0);
                                tienda.CodigoLegado = reader.GetString(1);
                                tienda.DescripcionTienda = reader.GetString(2);
                                tiendas.Add(tienda);
                            }
                        }
                    }
                    result.Correct = true;
                    result.Object = tiendas;
                }
            
            }
            catch(Exception ex)         {
            result.Correct = false;
            result.Message = "Error obteniendo tiendas: " + ex.Message;
            result.Ex = ex;
         }
            return result;
        }
        public static ML.Result GenerateCsvFile(ML.OrdenDummy.CapturaOrdenDummy ordenDummy, string destinationFolder)
        {
            var result = new ML.Result();
            try
            {
                if (ordenDummy == null)
                {
                    result.Correct = false;
                    result.Message = "Los datos de la orden son requeridos.";
                    return result;
                }

                if (string.IsNullOrWhiteSpace(ordenDummy.Orden) || string.IsNullOrWhiteSpace(ordenDummy.TiendaDestino) || ordenDummy.CantidadPiezas <= 0)
                {
                    result.Correct = false;
                    result.Message = "Orden, tienda destino y cantidad deben ser valores v�lidos.";
                    return result;
                }

                Directory.CreateDirectory(destinationFolder);

                var fileName = $"SE_ORDENTIE_{DateTime.Now:ddMMyyyyHHmm}.csv";
                var filePath = Path.Combine(destinationFolder, fileName);

                var today = DateTime.Now.ToString("yyyyMMdd");

                var line1 = BuildCsvLine(
                    "C",
                    ordenDummy.Orden,
                    "110403",
                    "GPOSAN",
                    ordenDummy.Orden,
                    "TRA_TIE",
                    today,
                    string.Empty,
                    today,
                    ordenDummy.TiendaDestino,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "CREATE",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    ordenDummy.TiendaDestino,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "SRS306",
                    "NO",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "SEARS",
                    "200",
                    "Mexico/General",
                    "TRANSFER_TIE_TIE_2",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "NA",
                    "NA",
                    "NA",
                    "NA",
                    "NA"
                );

                var line2 = BuildCsvLine(
                    "D",
                    ordenDummy.Orden,
                    "110403",
                    "GPOSAN",
                    ordenDummy.Orden,
                    "1",
                    string.Empty,
                    "SRS10291428",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    ordenDummy.CantidadPiezas.ToString(),
                    string.Empty,
                    string.Empty,
                    "CREATE",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "21289.06",
                    "35999.00",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "MXN",
                    "MXN",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "SRS1005077",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "SEARS"
                );

                WriteLinesToCsv(filePath, new[] { line1, line2 });

                result.Correct = true;
                result.Object = filePath;
                result.Message = "Archivo CSV generado correctamente.";
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "Error generando archivo CSV: " + ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        private static void WriteLinesToCsv(string filePath, IEnumerable<string> lines)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static string BuildCsvLine(params string[] fields)
        {
            return string.Join('|', fields);
        }
    }
}