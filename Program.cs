Console.WriteLine("Hello, World!");
﻿using System;
using Microsoft.Data.SqlClient;

Console.WriteLine("Bienvenido al sistema de Registro de Pacientes");

// conexion a base de datos
string connectionString = "Server=localhost;Database=DATABASEPACIENTES;Trusted_Connection=True;TrustServerCertificate=True;";

bool running = true;

while (running)
{
    Console.WriteLine("\n1. Registrar Paciente   2. Ver Pacientes   3. Salir");
    Console.Write("Seleccione una opción: ");

    int opcion;
    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        Console.WriteLine("Debes ingresar un número.");
        continue;
    }

    switch (opcion)
    {
        case 1:
            AddPaciente(connectionString);
            break;

        case 2:
            VerPacientes(connectionString);
            break;

        case 3:
            running = false;
            Console.WriteLine("Saliendo...");
            break;

        default:
            Console.WriteLine("Opción inválida.");
            break;
    }
}

// AGREGAR PACIENTE
static void AddPaciente(string connectionString)
{
    Console.Write("Nombre: ");
    string nombre = Console.ReadLine();

    Console.Write("Edad: ");
    int edad;
    if (!int.TryParse(Console.ReadLine(), out edad))
    {
        Console.WriteLine("Edad inválida.");
        return;
    }

    Console.Write("Motivo de visita: ");
    string motivo = Console.ReadLine();

    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string query = "INSERT INTO Pacientes (Nombre, Edad, Motivo) VALUES (@n, @e, @m)";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@e", edad);
            cmd.Parameters.AddWithValue("@m", motivo);

            cmd.ExecuteNonQuery();
        }

        Console.WriteLine("Paciente guardado en SQL Server");
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR SQL: " + ex.Message);
    }
}

// VER PACIENTES
static void VerPacientes(string connectionString)
{
    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string query = "SELECT Nombre, Edad, Motivo FROM Pacientes";
            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\n--- LISTA DE PACIENTES ---");
            Console.WriteLine("Nombre\t\tEdad\tMotivo");
            Console.WriteLine("-----------------------------------");

            while (reader.Read())
            {
                Console.WriteLine($"{reader["Nombre"]}\t\t{reader["Edad"]}\t{reader["Motivo"]}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR SQL: " + ex.Message);
    }
}