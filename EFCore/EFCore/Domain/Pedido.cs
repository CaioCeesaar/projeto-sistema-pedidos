﻿using EFCore.ValueObjects;

namespace EFCore.Domain;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public DateTime IniciadoEm { get; set; }
    public DateTime FinalizadoEm { get; set; }
    public StatusPedidos Status { get; set; }
    public TipoFrete TipoFrete { get; set; }
    public string Observacao { get; set; }
    public ICollection<PedidoItem> Itens { get; set; }
}