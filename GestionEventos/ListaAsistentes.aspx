<%@ Page Title="Lista de Asistentes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListaAsistentes.aspx.cs" Inherits="GestionEventos.ListaAsistentes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="color: #d81b60;">Lista de Asistencia por Evento</h2>
    <hr />

    <div style="display:flex; gap:10px; margin-bottom:20px;">
        <asp:DropDownList ID="ddlEventosLista" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="btnBuscar_Click"></asp:DropDownList>
        <asp:Button ID="btnBuscar" runat="server" Text="Actualizar Tabla" OnClick="btnBuscar_Click" CssClass="btn-pink" />
    </div>

    <asp:GridView ID="gridAsistentes" runat="server" AutoGenerateColumns="False" 
        CssClass="table" Width="100%" GridLines="None" 
        EmptyDataText="No hay alumnos inscritos en este evento.">
        <HeaderStyle BackColor="#ff4081" ForeColor="White" Height="40px" />
        <RowStyle BackColor="#f9f9f9" Height="35px" />
        <Columns>
            <asp:BoundField DataField="id_institucional" HeaderText="Matrícula" />
            <asp:BoundField DataField="nombre_completo" HeaderText="Nombre" />
            <asp:BoundField DataField="correo" HeaderText="Correo" />
        </Columns>
    </asp:GridView>
</asp:Content>
