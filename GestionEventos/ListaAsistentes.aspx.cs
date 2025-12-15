using System;
using System.Data;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace GestionEventos
{
    /// <summary>
    /// Clase que gestiona la página para mostrar la lista de asistentes a un evento.
    /// </summary>
    public partial class ListaAsistentes : System.Web.UI.Page
    {
        // Cadena de conexión a la base de datos MySQL. **IMPORTANTE:** ¡No subir contraseñas a repositorios públicos!
        string connStr = "Server=localhost;Database=congreso_mercadotecnias;Uid=root;Pwd=1234;";

        /// <summary>
        /// Evento que se ejecuta al cargar la página.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verifica si la página se está cargando por primera vez.
            if (!IsPostBack)
            {
                // Si es la primera carga, se carga el dropdownlist con los eventos.
                CargarDropdown();
            }
        }

        /// <summary>
        /// Carga los eventos desde la base de datos en el dropdownlist 'ddlEventosLista'.
        /// </summary>
        private void CargarDropdown()
        {
            // 'using' asegura que la conexión se cierre correctamente, incluso si hay errores.
            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                // Abre la conexión a la base de datos.
                con.Open();
                // Define la consulta SQL para seleccionar el ID y la actividad de todos los eventos.
                string query = "SELECT id, actividad FROM eventos";
                // Crea un DataAdapter para ejecutar la consulta y llenar un DataTable.
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                // Llena el DataTable con los resultados de la consulta.
                da.Fill(dt);
                // Asigna el DataTable como origen de datos para el dropdownlist 'ddlEventosLista'.
                ddlEventosLista.DataSource = dt;
                // Especifica qué columna del DataTable se usará para mostrar el texto en el dropdownlist.
                ddlEventosLista.DataTextField = "actividad";
                // Especifica qué columna del DataTable se usará como valor para cada opción en el dropdownlist.
                ddlEventosLista.DataValueField = "id";
                // Enlaza los datos al dropdownlist.
                ddlEventosLista.DataBind();
                // Inserta una opción por defecto en el dropdownlist para que el usuario seleccione un evento.
                ddlEventosLista.Items.Insert(0, new ListItem("-- Seleccione Evento --", "0"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón 'btnBuscar'.
        /// Muestra la lista de asistentes al evento seleccionado en el GridView 'gridAsistentes'.
        /// </summary>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // 'using' asegura que la conexión se cierre correctamente, incluso si hay errores.
            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                // Abre la conexión a la base de datos.
                con.Open();
                // Define la consulta SQL para seleccionar la matrícula, el nombre completo y el correo electrónico de los asistentes al evento seleccionado.
                // Realiza un JOIN entre las tablas 'inscripciones' y 'usuarios' para obtener la información de los usuarios inscritos en el evento.
                string query = @"SELECT u.id_institucional, CONCAT(u.nombre, ' ', u.apellido_paterno) as nombre_completo, u.correo 
                                 FROM inscripciones i 
                                 JOIN usuarios u ON i.usuario_id = u.id 
                                 WHERE i.evento_id = @eid";
                // Crea un SqlCommand para ejecutar la consulta.
                MySqlCommand cmd = new MySqlCommand(query, con);
                // Agrega el parámetro '@eid' con el valor del evento seleccionado en el dropdownlist.
                cmd.Parameters.AddWithValue("@eid", ddlEventosLista.SelectedValue);

                // Crea un DataAdapter para ejecutar la consulta y llenar un DataTable.
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                // Llena el DataTable con los resultados de la consulta.
                da.Fill(dt);
                // Asigna el DataTable como origen de datos para el GridView 'gridAsistentes'.
                gridAsistentes.DataSource = dt;
                // Enlaza los datos al GridView, haciendo que se muestren en la página.
                gridAsistentes.DataBind();
            }
        }
    }
}