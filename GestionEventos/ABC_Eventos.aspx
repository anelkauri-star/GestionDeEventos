<%@ Page Title="Administrar Eventos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABC_Eventos.aspx.cs" Inherits="GestionEventos.ABC_Eventos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="text-align: center; color: #555;">Administrar Programa de Eventos</h1>

    <asp:HiddenField ID="hfEventoId" runat="server" />

    <div style="background: #fff; padding: 20px; border-radius: 8px; margin-bottom: 30px;">
        <h2 style="color: #ff4081;">
            <asp:Label ID="lblTituloFormulario" runat="server" Text="Agregar Nuevo Evento"></asp:Label>
        </h2>
        
        <label>Actividad:</label>
        <asp:TextBox ID="txtActividad" runat="server" CssClass="form-control"></asp:TextBox>
        
        <div style="display:flex; gap:10px;">
            <div style="flex:1;"><label>Fecha:</label><asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox></div>
            <div style="flex:1;"><label>Hora Inicio:</label><asp:TextBox ID="txtInicio" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox></div>
            <div style="flex:1;"><label>Hora Fin:</label><asp:TextBox ID="txtFin" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox></div>
        </div>

        <label>Lugar:</label>
        <asp:TextBox ID="txtLugar" runat="server" CssClass="form-control"></asp:TextBox>

        <label>Tipo:</label>
        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
            <asp:ListItem>Conferencia</asp:ListItem>
            <asp:ListItem>Taller</asp:ListItem>
            <asp:ListItem>Registro</asp:ListItem>
            <asp:ListItem>Break</asp:ListItem>
        </asp:DropDownList>

        <label>Responsable:</label>
        <asp:TextBox ID="txtResponsable" runat="server" CssClass="form-control"></asp:TextBox>

        <div style="display:flex; gap:10px;">
            <asp:Button ID="btnGuardar" runat="server" Text="Agregar Evento" OnClick="btnGuardar_Click" CssClass="btn-pink" Width="100%" />
            
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar Edición" OnClick="btnCancelar_Click" CssClass="btn-dark" Visible="false" />
        </div>
        
        <br />
        <asp:Label ID="lblMensajeEvento" runat="server" Font-Bold="true"></asp:Label>
    </div>

    <div style="background: #fff5f8; padding: 20px; border: 2px dashed #ff4081; border-radius: 8px;">
        <h2 style="color: #d81b60;">Asignar Alumno a Evento</h2>
        <label>Seleccione Evento:</label>
        <asp:DropDownList ID="ddlEventosExistentes" runat="server" CssClass="form-control"></asp:DropDownList>
        <label>Matrícula del Alumno:</label>
        <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" placeholder="Ej: UP200198"></asp:TextBox>
        <asp:Button ID="btnInscribir" runat="server" Text="Registrar Alumno" OnClick="btnInscribir_Click" CssClass="btn-dark" Width="100%" />
        <br /><br />
        <asp:Button ID="btnIrALista" runat="server" Text="Ver Alumnos Registrados (Lista)" PostBackUrl="~/ListaAsistentes.aspx" CssClass="btn-pink" Width="100%" />
        <br /><br />
        <asp:Label ID="lblMensajeInscripcion" runat="server"></asp:Label>
    </div>

    <h2 style="margin-top: 40px; text-align:center;">Eventos Actuales</h2>
    
    <asp:Repeater ID="rptEventos" runat="server" OnItemCommand="rptEventos_ItemCommand">
        <ItemTemplate>
            <div style="background: #f9f9f9; padding: 15px; margin-bottom: 10px; border-left: 5px solid #ff4081; display:flex; justify-content:space-between; align-items:center;">
                <div>
                    <h3 style="margin:0; color:#333;"><%# Eval("actividad") %></h3>
                    <p style="margin:0; color:#666;">
                        <%# Convert.ToDateTime(Eval("fecha")).ToString("dd/MM/yyyy") %> | 
                        <%# Eval("hora_inicio") %> - <%# Eval("hora_fin") %>
                    </p>
                </div>
                <div>
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Editar" CommandArgument='<%# Eval("id") %>' 
                        CssClass="btn-dark" style="background-color: #ff9800; border:none; margin-right:5px;" />

                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("id") %>' 
                        CssClass="btn-dark" style="background-color:#dc3545;" OnClientClick="return confirm('¿Borrar evento?');" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>
