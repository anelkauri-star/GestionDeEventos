using System;
using System.Data;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace GestionEventos
{
    /// <summary>
    /// Clase que gestiona la página de administración de eventos (ABC - Altas, Bajas, Cambios).
    /// Permite crear, editar, eliminar eventos y asignar alumnos a eventos.
    /// </summary>
    public partial class ABC_Eventos : System.Web.UI.Page
    {
        // Cadena de conexión a la base de datos MySQL.  **IMPORTANTE:** ¡No subir contraseñas a repositorios públicos!
        string connStr = "Server=localhost;Database=congreso_mercadotecnias;Uid=root;Pwd=1234;";

        /// <summary>
        /// Evento que se ejecuta al cargar la página.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // IsPostBack se usa para verificar si la página se está cargando por primera vez o si es una respuesta a un evento (como un clic de botón).
            if (!IsPostBack)
            {
                // Si es la primera carga, se cargan los eventos desde la base de datos.
                CargarEventos();
            }
        }

        /// <summary>
        /// Carga los eventos desde la base de datos y los muestra en el repeater 'rptEventos' y en el dropdownlist 'ddlEventosExistentes'.
        /// </summary>
        private void CargarEventos()
        {
            // 'using' asegura que la conexión se cierre correctamente, incluso si hay errores.
            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                // Abre la conexión a la base de datos.
                con.Open();
                // Define la consulta SQL para seleccionar todos los eventos, ordenados por fecha descendente.
                string query = "SELECT * FROM eventos ORDER BY fecha DESC";
                // Crea un DataAdapter para ejecutar la consulta y llenar un DataTable.
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                // Llena el DataTable con los resultados de la consulta.
                da.Fill(dt);

                // Asigna el DataTable como origen de datos para el repeater 'rptEventos'.
                rptEventos.DataSource = dt;
                // Enlaza los datos al repeater, haciendo que se muestren en la página.
                rptEventos.DataBind();

                // Asigna el mismo DataTable como origen de datos para el dropdownlist 'ddlEventosExistentes'.
                ddlEventosExistentes.DataSource = dt;
                // Especifica qué columna del DataTable se usará para mostrar el texto en el dropdownlist.
                ddlEventosExistentes.DataTextField = "actividad";
                // Especifica qué columna del DataTable se usará como valor para cada opción en el dropdownlist.
                ddlEventosExistentes.DataValueField = "id";
                // Enlaza los datos al dropdownlist.
                ddlEventosExistentes.DataBind();
                // Inserta una opción por defecto en el dropdownlist para que el usuario seleccione un evento.
                ddlEventosExistentes.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón 'btnGuardar'.
        /// Permite insertar un nuevo evento o actualizar uno existente.
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    string sql = "";

                    // Determina si se está insertando un nuevo evento o actualizando uno existente.
                    if (string.IsNullOrEmpty(hfEventoId.Value))
                    {
                        // INSERTAR NUEVO EVENTO
                        sql = @"INSERT INTO eventos (fecha, hora_inicio, hora_fin, lugar, tipo_evento, conferencista_responsable, actividad) 
                                VALUES (@fecha, @inicio, @fin, @lugar, @tipo, @resp, @actividad)";
                    }
                    else
                    {
                        // ACTUALIZAR EVENTO EXISTENTE
                        sql = @"UPDATE eventos SET 
                                fecha=@fecha, hora_inicio=@inicio, hora_fin=@fin, lugar=@lugar, 
                                tipo_evento=@tipo, conferencista_responsable=@resp, actividad=@actividad 
                                WHERE id=@id";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Si se está actualizando un evento, se agrega el parámetro '@id' con el valor del HiddenField 'hfEventoId'.
                        if (!string.IsNullOrEmpty(hfEventoId.Value))
                        {
                            cmd.Parameters.AddWithValue("@id", hfEventoId.Value);
                        }

                        // Agrega los parámetros con los valores de los controles del formulario.
                        cmd.Parameters.AddWithValue("@fecha", txtFecha.Text);
                        cmd.Parameters.AddWithValue("@inicio", txtInicio.Text);
                        cmd.Parameters.AddWithValue("@fin", txtFin.Text);
                        cmd.Parameters.AddWithValue("@lugar", txtLugar.Text);
                        cmd.Parameters.AddWithValue("@tipo", ddlTipo.SelectedValue);
                        cmd.Parameters.AddWithValue("@resp", txtResponsable.Text);
                        cmd.Parameters.AddWithValue("@actividad", txtActividad.Text);
                        // Ejecuta la consulta SQL.
                        cmd.ExecuteNonQuery();
                    }
                }

                // Muestra un mensaje de éxito al usuario.
                if (string.IsNullOrEmpty(hfEventoId.Value))
                    lblMensajeEvento.Text = "Evento creado correctamente.";
                else
                    lblMensajeEvento.Text = "Evento actualizado correctamente.";

                lblMensajeEvento.ForeColor = System.Drawing.Color.Green;

                // Limpia el formulario y recarga los eventos.
                LimpiarFormulario();
                CargarEventos();
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error al usuario si ocurre una excepción.
                lblMensajeEvento.Text = "Error: " + ex.Message;
                lblMensajeEvento.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en un botón de comando en el repeater 'rptEventos' (Editar o Eliminar).
        /// </summary>
        protected void rptEventos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Obtiene el ID del evento del CommandArgument del botón.
            int idEvento = Convert.ToInt32(e.CommandArgument);

            // Determina qué comando se ejecutó (Editar o Eliminar).
            if (e.CommandName == "Eliminar")
            {
                // Lógica para eliminar el evento.
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    // **IMPORTANTE: INTEGRIDAD REFERENCIAL**
                    // Borra primero las inscripciones asociadas al evento para evitar errores de clave foránea.
                    string sqlDelInsc = "DELETE FROM inscripciones WHERE evento_id = @id";
                    new MySqlCommand(sqlDelInsc, con) { Parameters = { new MySqlParameter("@id", idEvento) } }.ExecuteNonQuery();

                    // Borra el evento.
                    string sqlDelEvento = "DELETE FROM eventos WHERE id = @id";
                    new MySqlCommand(sqlDelEvento, con) { Parameters = { new MySqlParameter("@id", idEvento) } }.ExecuteNonQuery();
                }
                // Recarga los eventos y limpia el formulario.
                CargarEventos();
                LimpiarFormulario();
            }
            else if (e.CommandName == "Editar")
            {
                // Lógica para cargar los datos del evento en el formulario para editar.
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    string query = "SELECT * FROM eventos WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", idEvento);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Llena los campos del formulario con los datos del evento.
                            txtActividad.Text = reader["actividad"].ToString();
                            txtLugar.Text = reader["lugar"].ToString();
                            txtResponsable.Text = reader["conferencista_responsable"].ToString();

                            // Formatea la fecha para mostrarla correctamente en el TextBox.
                            txtFecha.Text = Convert.ToDateTime(reader["fecha"]).ToString("yyyy-MM-dd");
                            txtInicio.Text = reader["hora_inicio"].ToString();
                            txtFin.Text = reader["hora_fin"].ToString();

                            ddlTipo.SelectedValue = reader["tipo_evento"].ToString();

                            // Guarda el ID del evento en el HiddenField 'hfEventoId'.
                            hfEventoId.Value = idEvento.ToString();

                            // Cambia el texto del botón 'btnGuardar' y del label 'lblTituloFormulario' para indicar que se está editando un evento.
                            btnGuardar.Text = "Actualizar Evento";
                            lblTituloFormulario.Text = "Editando: " + reader["actividad"].ToString();
                            // Muestra el botón 'btnCancelar'.
                            btnCancelar.Visible = true;

                            // Mueve el foco al principio del formulario
                            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#top';", true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón 'btnCancelar'.
        /// Limpia el formulario y restablece su estado original.
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        /// <summary>
        /// Limpia los campos del formulario y restablece su estado original.
        /// </summary>
        private void LimpiarFormulario()
        {
            txtActividad.Text = "";
            txtFecha.Text = "";
            txtInicio.Text = "";
            txtFin.Text = "";
            txtLugar.Text = "";
            txtResponsable.Text = "";
            ddlTipo.SelectedIndex = 0;

            // Restablece el estado original del formulario.
            hfEventoId.Value = ""; // Borra el ID oculto
            btnGuardar.Text = "Agregar Evento";
            lblTituloFormulario.Text = "Agregar Nuevo Evento";
            btnCancelar.Visible = false;
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón 'btnInscribir'.
        /// Inscribe a un alumno a un evento.
        /// </summary>
        protected void btnInscribir_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    // Busca el ID del alumno en la tabla 'usuarios' usando la matrícula ingresada.
                    string sqlBuscar = "SELECT id FROM usuarios WHERE id_institucional = @matricula";
                    MySqlCommand cmdBuscar = new MySqlCommand(sqlBuscar, con);
                    cmdBuscar.Parameters.AddWithValue("@matricula", txtMatricula.Text);

                    object resultado = cmdBuscar.ExecuteScalar();

                    if (resultado != null)
                    {
                        // Si se encuentra el alumno, se obtiene su ID.
                        int idAlumno = Convert.ToInt32(resultado);
                        // Inserta una nueva inscripción en la tabla 'inscripciones'.
                        string sqlInsert = "INSERT INTO inscripciones (usuario_id, evento_id) VALUES (@uid, @eid)";
                        MySqlCommand cmdInsert = new MySqlCommand(sqlInsert, con);
                        cmdInsert.Parameters.AddWithValue("@uid", idAlumno);
                        cmdInsert.Parameters.AddWithValue("@eid", ddlEventosExistentes.SelectedValue);
                        cmdInsert.ExecuteNonQuery();

                        // Muestra un mensaje de éxito al usuario.
                        lblMensajeInscripcion.Text = "Alumno inscrito con éxito.";
                        lblMensajeInscripcion.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        // Si no se encuentra el alumno, muestra un mensaje de error.
                        lblMensajeInscripcion.Text = "Alumno no encontrado.";
                        lblMensajeInscripcion.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error al usuario si ocurre una excepción.
                lblMensajeInscripcion.Text = "Error: " + ex.Message;
            }
        }
    }
}