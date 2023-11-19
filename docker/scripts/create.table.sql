/*!40101 SET NAMES utf8 */;
/*!40014 SET FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET SQL_NOTES=0 */;
DROP TABLE IF EXISTS `Autorizacion`;
CREATE TABLE `Autorizacion` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Escasez_prod_id` int NOT NULL,
  `Usuario_id` int NOT NULL,
  `Fecha_autoriza` datetime NOT NULL,
  `Estado_autoriza` int NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=62 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `Cte_prov`;
CREATE TABLE `Cte_prov` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `NombreEmpresa` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Contacto` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Telefono` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CorreoElectronico` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Direccion` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `Escasez_Producto`;
CREATE TABLE `Escasez_Producto` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ProductoId` int NOT NULL,
  `Cant_Soli` int NOT NULL,
  `Fecha_Registro` datetime NOT NULL,
  `UsuarioId` int NOT NULL,
  `Estatus` int DEFAULT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `Observacion` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `Estatus`;
CREATE TABLE `Estatus` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Estatus` varchar(20) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `Producto`;
CREATE TABLE `Producto` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Nom_prod` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Desc_prod` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Categoria` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Precio` decimal(10,2) DEFAULT NULL,
  `Stock` int DEFAULT NULL,
  `Cve_prov` int NOT NULL DEFAULT '0',
  `Estatus` int NOT NULL,
  `Imagen` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `RefreshToken`;
CREATE TABLE `RefreshToken` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `UsuarioId` int NOT NULL,
  `Token` longtext,
  `Expires` datetime(6) NOT NULL,
  `Created` datetime(6) NOT NULL,
  `Revoked` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `UsuarioId` (`UsuarioId`),
  CONSTRAINT `RefreshToken_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuario` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `Reporte`;
CREATE TABLE `Reporte` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `TipoReporte` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FechaGeneracion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `Cve_usr` int NOT NULL DEFAULT '0',
  `DatosReporte` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `Rol`;
CREATE TABLE `Rol` (
  `ID` int NOT NULL,
  `Nombre` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `Usuario`;
CREATE TABLE `Usuario` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Nom_user` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Apellido` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CorreoElectronico` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Jefe_id` int DEFAULT NULL,
  `Permiso` int NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `CorreoElectronico` (`CorreoElectronico`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `UsuarioRoles`;
CREATE TABLE `UsuarioRoles` (
  `UsuarioId` int DEFAULT NULL,
  `RolId` int DEFAULT NULL,
  KEY `UsuarioId` (`UsuarioId`),
  KEY `RolId` (`RolId`),
  CONSTRAINT `UsuarioRoles_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuario` (`ID`),
  CONSTRAINT `UsuarioRoles_ibfk_2` FOREIGN KEY (`RolId`) REFERENCES `Rol` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `Autorizacion`(ID,Escasez_prod_id,Usuario_id,Fecha_autoriza,Estado_autoriza) VALUES('1','2','9','\'2023-11-02 20:45:38\'','7'),('4','2','9','\'2023-11-02 20:45:38\'','7'),('5','2','9','\'2023-11-02 20:45:38\'','7'),('6','2','9','\'2023-11-02 20:45:38\'','7'),('7','2','9','\'2023-11-02 20:45:38\'','7'),('8','2','9','\'2023-11-02 20:45:38\'','7'),('9','2','9','\'2023-11-02 20:45:38\'','7'),('25','2','9','\'2023-11-02 20:45:38\'','7'),('31','2','9','\'2023-11-02 20:45:38\'','7'),('32','2','9','\'2023-11-02 20:45:38\'','7'),('33','2','9','\'2023-11-02 20:45:38\'','7'),('34','2','9','\'2023-11-02 20:45:38\'','7'),('35','2','9','\'2023-11-02 20:45:38\'','7'),('43','2','9','\'2023-11-02 20:45:38\'','7'),('44','2','9','\'2023-11-02 20:45:38\'','7'),('47','2','9','\'2023-11-02 20:45:38\'','7'),('53','2','9','\'2023-11-02 20:45:38\'','7'),('54','2','9','\'2023-11-02 20:45:38\'','7'),('55','2','9','\'2023-11-02 20:45:38\'','7'),('57','2','9','\'2023-11-02 20:45:38\'','7'),('58','2','9','\'2023-11-02 20:45:38\'','7'),('59','2','9','\'2023-11-02 20:45:38\'','7'),('60','2','9','\'2023-11-02 20:45:38\'','7'),('61','2','9','\'2023-11-02 20:45:38\'','7');

INSERT INTO `Cte_prov`(ID,NombreEmpresa,Contacto,Telefono,CorreoElectronico,Direccion) VALUES('1','\'Adidas\'','\'Juan Pérez\'','\'123456789\'','\'juan@example.com\'','\'123 Calle Principal\''),('2','\'Nike\'','\'Jose Mendoza\'','\'1234567805\'','\'mendoza@example.com\'','\'589 Calle Principal\''),('3','\'H&M\'','\'Emmanuel Martinez\'','\'9874562364\'','\'jairo@example.com\'','\'AL. 256 CALLE\''),('4','\'Levis\'','\'Fernando Duarte\'','\'4561237896\'','\'fer@gmail.com\'','\'Lops 4589\''),('5','\'Hollister\'','\'Juan Jose Jimenez\'','\'7894561230\'','\'jimenez@gmail.com\'','\'GHolo 752\''),('6','\'CalvinKlein\'','\'Travis Scoot\'','\'4567891230\'','\'skot@gmail.com\'','\'Fer848x\''),('11','\'Gucci\'','\'Sandel Martinez\'','\'9874562368\'','\'sandel@example.com\'','\'AL. 259 CALLE\''),('100','\'Lacoste\'','\'Gerardo Velázquez\'','\'1234566789\'','\'gera@gmail.com\'','\'gakha\''),('103','\'Pirma\'','\'Hugo Sánchez\'','\'4561237890\'','\'hugo@gmail.com\'','\'Zanahoria Av.\''),('104','\'Hermes Paris\'','\'Frank Verti\'','\'427896563\'','\'frank@gmail.com\'','\'Paris 698. mex\''),('105','\'Channel\'','\'Maria Vendeti\'','\'4567891230\'','\'vendi78@gmail.com\'','\'Vendi 789 lop\''),('106','\'Luis Vuitton\'','\'Jose Juan Duarte\'','\'3219876540\'','\'juanjos8@gmail.com\'','\'Carbon 789\''),('107','\'GAP\'','\'Diego Nieto\'','\'3697412589\'','\'niu89@gmail.com\'','\'Netra 7895 l\'');

INSERT INTO `Escasez_Producto`(ID,ProductoId,Cant_Soli,Fecha_Registro,UsuarioId,Estatus,Precio,Observacion) VALUES('2','1','20','\'2023-10-21 19:13:42\'','9','7','1500','NULL'),('3','1','40','\'2023-10-21 20:49:10\'','9','4','6000','NULL'),('4','1','40','\'2023-10-22 14:02:57\'','9','4','6000','NULL'),('5','1','40','\'2023-10-22 16:34:12\'','9','4','6000','NULL'),('6','1','40','\'2023-11-01 20:45:38\'','9','4','6000','NULL');

INSERT INTO `Estatus`(ID,Estatus) VALUES('1','\'Disponible\''),('2','\'Por Terminado\''),('3','\'No Disponible\''),('4','\'Pendiente\''),('5','\'Rechazado\''),('6','\'Por Entregar\''),('7','\'Autorizado\'');

INSERT INTO `Producto`(ID,Nom_prod,Desc_prod,Categoria,Precio,Stock,Cve_prov,Estatus,Imagen) VALUES('1','\'Playera Manga Larga\'','X\'506c6179657261\'','\'Playera\'','150.00','20','3','6','\'https://images.vexels.com/media/users/3/153207/isolated/preview/52448e896a77b1424a70c8f6ce32e310-icono-de-trazo-de-camiseta-de-manga-larga.png\''),('2','\'Pantalon de mezclilla\'','X\'50616e74616c6f6e\'','\'Pantalon\'','350.00','10','4','2','\'https://westernnava.mx/pub/media/catalog/product/cache/14464cfce3c0cb7d83577c49cefcf701/5/0/501011516006_1.png\''),('3','\'Ropa interior de caballero\'','X\'526f706120496e746572696f72\'','\'Ropa Interior\'','100.00','20','6','1','\'https://i.pinimg.com/originals/91/b4/63/91b463c7fa8796fd8351cc8ef7977fe1.png\''),('4','\'Tenis Nike\'','X\'54656e6973206465206c61206d61726361204e696b65\'','\'Calzado\'','29.99','50','2','1','\'https://www.debate.com.mx/__export/1553885999184/sites/debate/img/2019/03/29/1_1.png_458618655.png\''),('5','\'Sudadera Negra\'','X\'5375646164657261206e65677261206465206c61206d61726361204c65766973\'','\'Sudadera\'','49.99','30','4','2','\'https://loladetalles.com/cdn/shop/products/image_caf77102-1b73-41b3-823e-2556a4fb988a_grande.png?v=1661865554\''),('6','\'Playera Polo\'','X\'506c617965726120506f6c6f206465206c61206d617263612043616c76696e4b6c65696e\'','\'Playera\'','19.99','80','6','3','\'https://agdistribuidora.net/cdn/shop/files/PhotoRoom-20230724_102619.png?v=1690219666\''),('7','\'Playera Negra\'','X\'506c6179657261204775636369\'','\'Playera\'','99.99','20','11','4','\'https://i.pinimg.com/originals/9a/35/cb/9a35cb30b25c7f7e187060c08c368096.png\''),('8','\'Vestido Azul\'','X\'5665737469646f204368616e6e656c\'','\'Vestido\'','39.99','60','105','5','\'https://i.pinimg.com/736x/a0/73/4c/a0734ce11021e117e1a51f25f441f9cd.jpg\''),('9','\'Playera de México\'','X\'506c61796572612064652073656c65636369c3b36e\'','\'Deporte\'','79.99','45','1','6','\'https://i.pinimg.com/736x/b2/17/a0/b217a05f436f8e493d94a7329b462a06.jpg\''),('10','\'Mochila Gris\'','X\'4d6f6368696c61207061726120686163657220656a6572636963696f\'','\'Mochila\'','129.99','10','103','7','\'https://cdn.shopify.com/s/files/1/0386/5817/9204/products/8D2A9991_de52b068-fcb3-469e-9276-5994670ad192_2000x.jpg?v=1652732632\''),('11','\'Pantalon Verde\'','X\'50616e74616c6f6e205665726465\'','\'Pantalon\'','59.99','70','100','1','\'https://images.vestiairecollective.com/cdn-cgi/image/w=1246,q=70,f=auto,/produit/pantalon-lacoste-de-sintetico-verde-15337336-1_3.jpg\''),('12','\'Sudadera Azul\'','X\'537564616465726120417a756c\'','\'Sudadera\'','89.99','25','5','2','\'https://i5.walmartimages.com.mx/mg/gm/3pp/asr/227083a5-cb11-4370-b5f8-d2914fbefb8c.e703db9cb5302d146671619407046dd8.jpeg?odnHeight=612&odnWidth=612&odnBg=FFFFFF\''),('13','\'Sudadera Roja\'','X\'537564616465726120526f6a61\'','\'Sudadera\'','149.99','15','107','3','\'https://i.pinimg.com/564x/a2/c9/2b/a2c92b30259a68c53b4b9f1de38ea42d.jpg\'');

INSERT INTO `RefreshToken`(ID,UsuarioId,Token,Expires,Created,Revoked) VALUES('1','5','X\'43374b775069694b387752744d337166386d5a2b613474565a67584e757348745033444a5351366c4475383d\'','\'2023-10-14 14:46:26.448759\'','\'2023-10-04 14:46:26.448827\'','\'2023-10-04 14:59:20.352648\''),('2','5','X\'636877747964334b7531365772566a4152317162736a7071314476496c584768315a636347762f4d6165413d\'','\'2023-10-14 14:59:20.353003\'','\'2023-10-04 14:59:20.353042\'','\'2023-10-12 04:17:59.616425\''),('3','5','X\'4456676578754455584a724b70553932726c484e746a61594242375338566c6147506d43442f76543253513d\'','\'2023-10-22 04:17:59.616970\'','\'2023-10-12 04:17:59.617026\'','\'2023-10-12 04:21:06.350530\''),('4','5','X\'503633556f62636446763177577a5066777a32707134754b6948685a31432f796c66695271382b42516c4d3d\'','\'2023-10-22 04:21:06.351048\'','\'2023-10-12 04:21:06.351089\'','\'2023-10-12 16:02:14.747767\''),('5','5','X\'5a2f364349517134575930706b752b6d636a372f7756304f6f49763556366e35756a424f367065455546513d\'','\'2023-10-22 16:02:14.748386\'','\'2023-10-12 16:02:14.748450\'','\'2023-10-12 16:36:08.834147\''),('6','5','X\'4656367749334f5947437257436250487941546b6458654136734f334c345535422f6e58796130425254593d\'','\'2023-10-22 16:36:08.834516\'','\'2023-10-12 16:36:08.834553\'','\'2023-10-13 00:15:26.096632\''),('7','5','X\'473379516575577a6f53303878486a4a317130625168494c517831594b7379666e646b4945316c665274493d\'','\'2023-10-23 00:15:26.096995\'','\'2023-10-13 00:15:26.097053\'','\'2023-10-17 04:12:26.079575\''),('8','5','X\'4974513635714d734a74546976555a5467367638574657476162466f67415867736f6161572f67545039633d\'','\'2023-10-27 04:12:26.080123\'','\'2023-10-17 04:12:26.080180\'','NULL'),('9','5','X\'69594b615067507a4e6e38474f6a445a61374c447a4b58627a356a726e35726f7a563634566e7a727561513d\'','\'2023-11-06 04:26:57.124512\'','\'2023-10-27 04:26:57.124566\'','NULL'),('10','5','X\'7a67433037544535574447706d52383646425a7935486a61506957354835316a552f68354442696e6946303d\'','\'2023-11-18 03:51:31.649798\'','\'2023-11-08 03:51:31.649854\'','NULL');


INSERT INTO `Rol`(ID,Nombre) VALUES('1','\'Administrador\''),('2','\'Gerente\''),('3','\'Empleado\'');

INSERT INTO `Usuario`(ID,Nom_user,Apellido,CorreoElectronico,Password,Jefe_id,Permiso) VALUES('5','\'Jose123\'','\'Jose Martinez Olvera\'','\'jose@gmail.com\'','\'AQAAAAEAACcQAAAAECVsEuaKlnHF2NNabDjRMW6h5ZaQrZPGY98grlLVeYRNiNR1TqefpdGuoB+6H9Rlwg==\'','9','1'),('6','\'Alan\'','\'Alan Martinez Olvera\'','\'alan@gmail.com\'','\'AQAAAAEAACcQAAAAEP4+QZpDjrgIeSn/7Z/iaWcSa8eWkCgzBUPuAayaENCgfdlwSW3O/PKcG8122uUiKQ==\'','9','1'),('8','\'Sandra4788\'','\'Sandra Herrera Olvera\'','\'sandra@gmail.com\'','\'Pa$$w0rd\'','NULL','1'),('9','\'Rodri12\'','\'Rodrigo Téllez Escobedo\'','\'rtellez476@gmail.com\'','\'AQAAAAEAACcQAAAAEKlvux7itY4QMzmT7A2LCLgLBj4I2sq0v2nhMiggfoY77qAOPNKVoGZ3vNM8mybOEw==\'','NULL','1'),('10','\'Luis4788\'','\'Luis Martinez Olvera\'','\'luis89@gmail.com\'','\'AQAAAAEAACcQAAAAEFXoEjbQRagh9Iih+Kop6Y6nz9lfCFRnv+IT/oPujCUpbWz8zDG06lcabS7/CeqKdw==\'','NULL','1'),('11','\'Momete4788\'','\'Noé Martinez Olvera\'','\'noe89@gmail.com\'','\'AQAAAAEAACcQAAAAEHjj1DdY6ZuTF1dmOVd9ubF3n9/5aT4wXMG0QdQGrJHBlUPX98O1qcj6IafYiFU/qQ==\'','NULL','1');

INSERT INTO `UsuarioRoles`(UsuarioId,RolId) VALUES('5','3'),('5','1'),('6','3'),('8','3'),('9','2'),('10','3'),('11','3');
INSERT INTO `__EFMigrationsHistory`(MigrationId,ProductVersion) VALUES('\'20230925041459_InitialCreate\'','\'6.0.4\''),('\'20230925050116_Configurations\'','\'6.0.4\'');