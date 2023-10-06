
INSERT INTO Vouchers

       (Id
       ,Codigo
       ,Percentual
       ,ValorDesconto
       ,Quantidade
       ,TipoDesconto
       ,DataCriacao
       ,DataUtilizacao
       ,DataValidade
       ,Ativo
       ,Utilizado)
       
 VALUES
 
       ('2B653827-D64F-449F-9F30-777BED1D9631',
       '150-0FF-GERAL',
       NULL,
       150,
       50,
       1,
       '2023-03-03',
       NULL,
       '2024-03-03',
       1,
       0);
GO

INSERT INTO Vouchers

       (Id
       ,Codigo
       ,Percentual
       ,ValorDesconto
       ,Quantidade
       ,TipoDesconto
       ,DataCriacao
       ,DataUtilizacao
       ,DataValidade
       ,Ativo
       ,Utilizado)
 VALUES
 
       ('A1FEA06B-A0BF-4D57-B6C9-CB50DCFEF26D',
       '50-0FF-GERAL',
       50,
       null,
       50,
       0,
       '2023-03-03',
       NULL,
       '2024-03-03',
       1,
       0);
GO

INSERT INTO Vouchers

       (Id
       ,Codigo
       ,Percentual
       ,ValorDesconto
       ,Quantidade
       ,TipoDesconto
       ,DataCriacao
       ,DataUtilizacao
       ,DataValidade
       ,Ativo
       ,Utilizado)
 VALUES
 
       ('54C063B2-67C7-44FE-8FB9-E2D201BD5E4F',
       '10-0FF-GERAL',
       10,
       null,
       50,
       0,
       '2023-03-03',
       NULL,
       '2024-03-03',
       1,
       0);
GO

SELECT * FROM VOUCHERS