ALTER TABLE [dbo].[laporan] WITH CHECK ADD FOREIGN KEY([id_outbond])
REFERENCES [dbo].[outbond] ([id_outbond])
