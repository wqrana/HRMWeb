﻿
--Enter the db name to set TRUSTWORTHY ON
ALTER DATABASE [TimeAideInternalUAT_07072021] SET TRUSTWORTHY ON
GO 
-- Change the owner of the db to sys user, if it's not synced during back-up restoration
EXEC dbo.sp_changedbowner 'sa'
GO
-- reconfigure the db to support clr object
EXEC sp_configure 'clr enabled';  
EXEC sp_configure 'clr enabled' , '1';  
RECONFIGURE;
GO
-- run below for creation of CLR function fn_CLREnDecriptSSN
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating CLR assemblies'
GO
--Assembly timeaide.clr, version=0.0.0.0, culture=neutral, publickeytoken=null, processorarchitecture=msil
CREATE ASSEMBLY [TimeAide.CLR]
AUTHORIZATION [dbo]
FROM 0x4d5a90000300000004000000ffff0000b800000000000000400000000000000000000000000000000000000000000000000000000000000000000000800000000e1fba0e00b409cd21b8014ccd21546869732070726f6772616d2063616e6e6f742062652072756e20696e20444f53206d6f64652e0d0d0a2400000000000000\
504500004c010300ed4129610000000000000000e00022200b013000000c000000060000000000006a2b0000002000000040000000000010002000000002000004000000000000000600000000000000008000000002000000000000030060850000100000100000000010000010000000000000100000000000000000000000\
182b00004f00000000400000b802000000000000000000000000000000000000006000000c000000e02900001c0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200000080000000000000000000000082000004800000000000000000000002e74657874000000\
780b000000200000000c000000020000000000000000000000000000200000602e72737263000000b80200000040000000040000000e0000000000000000000000000000400000402e72656c6f6300000c0000000060000000020000001200000000000000000000000000004000004200000000000000000000000000000000\
4c2b000000000000480000000200050014220000cc07000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001b3005008e00000001000011026f0600000a8d0c0000010a280700000a7e02000004161e6f0800000a6f0900000a8003\
000004730a00000a0b02720100007072050000706f0b00000a280c00000a0a730d00000a0c08077e030000047e010000046f0e00000a17730f00000a250616068e696f1000000a6f1100000a280700000a086f1200000a6f1300000a0dde09267e1400000a0dde00092a00000110000000000c0077830009050000011b300600\
7100000002000011280700000a7e02000004161e6f0800000a6f0900000a8003000004730a00000a0a280700000a026f0900000a0b730d00000a25067e030000047e010000046f1500000a17730f00000a250716078e696f1000000a6f1100000a6f1200000a281600000a0cde09267e1400000a0cde00082a00000001100000\
0000000066660009050000011e02281700000a2ab21e8d0c00000125d004000004281800000a800100000472090000708002000004168d0c00000180030000042a000000133002004400000003000011721d0000700a02721d000070281900000a2c2a03721f000070281a00000a2c090228020000060a2b1403722300007028\
1a00000a2c070228010000060a06731b00000a2a42534a4201000100000000000c00000076342e302e33303331390000000005006c000000a0020000237e00000c030000a803000023537472696e677300000000b40600002800000023555300dc060000100000002347554944000000ec060000e000000023426c6f62000000\
0000000002000001571502200900000000fa0133001600000100000016000000040000000400000006000000040000001b00000005000000030000000100000001000000020000000000c30101000000000006001e019f0206003e019f020600f5008c020f00bf02000006001003ee01060024026b030600e1016e000a000901\
3d020a008a01ce020600da009f0206009701ee0106005c01ee0106006101350306002703ee010600f5016b03060008026b030600d4016b030600e7016e000600a5006b03060001039f0206005303ee010600b600ee010000000033000000000001000100010010001902000015000100010001001000ec020000150004000500\
000100003c0000001500040007003100850092001100590361001100670392003301010096005020000000009600170399000100fc200000000096001f03990002008c2100000000861858020600030094210000000091185e029e000300c4210000000096005b00a20003008c21000000008618580206000500000001002f03\
000001002f0300000100850200000200c900090058020100110058020600190058020a004100580206005100580206005900a8011a0069002a001e0059009e0123006900e302290031005802060059009d002f0071006a013500390058020600790065023b008900580244009100d4004e008900b30106003900510356006900\
94015b005900a2036100790075023b0071007b016c00290058020600a10041037200590094037e00590088037e004900580284002e000b00a9002e001300b2002e001b00d10083002b00da00a0002300da00100064007a00702b0000040004800000000000000000000000000000000078000000040000000000000000000000\
89009400000000000400000000000000000000008900880000000000000000000036393946423239314146314236303332374238383037464443463338343139314230414642363331006765745f55544638003c4d6f64756c653e003c50726976617465496d706c656d656e746174696f6e44657461696c733e00666e5f434c\
52456e4465637269707453534e0053797374656d2e494f0054696d65416964652e434c520049560053797374656d2e44617461006d73636f726c6962005265706c6163650043727970746f53747265616d4d6f64650052756e74696d654669656c6448616e646c6500616374696f6e5479706500577269746500436f6d70696c\
657247656e6572617465644174747269627574650044656275676761626c654174747269627574650053716c46756e6374696f6e41747472696275746500436f6d70696c6174696f6e52656c61786174696f6e734174747269627574650052756e74696d65436f6d7061746962696c6974794174747269627574650042797465\
00456e636f64696e670046726f6d426173653634537472696e6700546f426173653634537472696e670053716c537472696e6700476574537472696e6700537562737472696e67006765745f4c656e67746800466c75736846696e616c426c6f636b0054696d65416964652e434c522e646c6c0043727970746f53747265616d\
004d656d6f727953747265616d0053797374656d0053796d6d6574726963416c676f726974686d004943727970746f5472616e73666f726d00456e6372797074696f6e0044455343727970746f5365727669636550726f7669646572004d6963726f736f66742e53716c5365727665722e536572766572002e63746f72002e63\
63746f7200437265617465446563727970746f7200437265617465456e63727970746f720073736e5374720053797374656d2e446961676e6f73746963730053797374656d2e52756e74696d652e436f6d70696c6572536572766963657300446562756767696e674d6f6465730053797374656d2e446174612e53716c547970\
65730047657442797465730055736572446566696e656446756e6374696f6e730052756e74696d6548656c70657273004f626a656374004465637279707400456e637279707400436f6e7665727400496e7075740053797374656d2e5465787400496e697469616c697a65417272617900546f417272617900456e6372797074\
696f6e4b6579006b65790053797374656d2e53656375726974792e43727970746f677261706879006f705f457175616c697479006f705f496e657175616c69747900456d707479000003200000032b000013210035003600320033006100230064006500000100034500000344000000915c54c5c810bd4884d819d64b334b6f\
000420010108032000010520010111110907041d051219121d0e0320000804000012350520020e08080520011d050e0520020e0e0e0500011d050e08200212411d051d050920030112491241114d072003011d0508080420001d050520010e1d0502060e07070312191d050e0500010e1d0507000201125511590307010e0500\
02020e0e042001010e08b77a5c561934e08903061d0502060a0400010e0e0300000106000211250e0e0801000800000000001e01000100540216577261704e6f6e457863657074696f6e5468726f77730108010002000000000004010000000000000000ed41296100000000020000001c010000fc290000fc0b000052534453\
4268e3020da2854789f4bd17ec24ebc001000000443a5c4964656e546563682d54696d65416964655765625c4465764f70735c5265706f5c54696d65416964652e434c525c6f626a5c52656c656173655c54696d65416964652e434c522e70646200000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
000000000000000000000000000000000000000000000000402b000000000000000000005a2b00000020000000000000000000000000000000000000000000004c2b0000000000000000000000005f436f72446c6c4d61696e006d73636f7265652e646c6c0000000000ff25002000101234567890abcdef0000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
000000000000000000000000000001001000000018000080000000000000000000000000000001000100000030000080000000000000000000000000000001000000000048000000584000005c02000000000000000000005c0234000000560053005f00560045005200530049004f004e005f0049004e0046004f0000000000\
bd04effe00000100000000000000000000000000000000003f000000000000000400000002000000000000000000000000000000440000000100560061007200460069006c00650049006e0066006f00000000002400040000005400720061006e0073006c006100740069006f006e00000000000000b004bc01000001005300\
7400720069006e006700460069006c00650049006e0066006f0000009801000001003000300030003000300034006200300000002c0002000100460069006c0065004400650073006300720069007000740069006f006e000000000020000000300008000100460069006c006500560065007200730069006f006e0000000000\
30002e0030002e0030002e003000000042001100010049006e007400650072006e0061006c004e0061006d0065000000540069006d00650041006900640065002e0043004c0052002e0064006c006c00000000002800020001004c006500670061006c0043006f0070007900720069006700680074000000200000004a001100\
01004f0072006900670069006e0061006c00460069006c0065006e0061006d0065000000540069006d00650041006900640065002e0043004c0052002e0064006c006c0000000000340008000100500072006f006400750063007400560065007200730069006f006e00000030002e0030002e0030002e003000000038000800\
010041007300730065006d0062006c0079002000560065007200730069006f006e00000030002e0030002e0030002e003000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
002000000c0000006c3b00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000

WITH PERMISSION_SET=UNSAFE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER ASSEMBLY [TimeAide.CLR]
ADD FILE FROM 
0x4d6963726f736f667420432f432b2b204d534620372e30300d0a1a445300000000020000020000001b000000880000000000000018000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
c0ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
380000feffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff\
0bca3101380000000010000000100000000000000f00ffff04000000ffff0300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0bca3101380000000010000000100000000000001000ffff04000000ffff0300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
942e3101ed412961010000004268e3020da2854789f4bd17ec24ebc000000000000000000100000001000000000000000000000000000000dc5133010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0bca310138000000001000000010000000000000ffffffff04000000ffff030000000000ffffffff00000000ffffffff00000000ffffffff000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0bca310138000000001000000010000000000000ffffffff04000000ffff030000000000ffffffff00000000ffffffff00000000ffffffff000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
f862513fc607d311905300c04fa302a1c4454b99e9e6d211903f00c04fa302a10b9d865a1166d311bd2a0000f80849bdec1618ff5eaa104d87f76f4963833460140000000000000044cfe75f40513604e855729ec25e187d247e42b4000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
00000000000000006f010000000000006f010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
feeffeef010000009000000000443a5c4964656e546563682d54696d65416964655765625c4465764f70735c5265706f5c54696d65416964652e434c525c434c5255736572446566696e656446756e632e63730000643a5c6964656e746563682d74696d65616964657765625c6465766f70735c7265706f5c74696d65616964\
652e636c725c636c7275736572646566696e656466756e632e63730004000000010000004800000049000000000000000300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
1be2300180000000661ea6bb7c9bd701010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000200000001000000020000000000000049000000280000001be23001b6d0da2f5c000000010000004800000049000000650000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
040000003a002a1100000000ac00000000000000440000000000000000000000050000062b0100000100000000666e5f434c52456e4465637269707453534e00160003110400000078000000440000002b010000010000001e0020110000000003000011000000000000000072657475726e53534e000000020006002e000404\
c93feac6b359d649bc250902bbabb460000000004d0044003200000004010000040100000c0000000100000602000600f2000000780000002b010000010001004400000000000000080000006c000000000000006b000080060000006c000080130000006e000080200000007000008027000000710000802900000072000080\
36000000740000803d0000007900008009001f0009001a000d002300110038000d000e00120028001100380009002900f400000008000000010000000000000008000000000000002400000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0400000032002a1100000000b4010000000000008e00000000000000000000000100000600000000010000000044656372797074000000001600031104000000800100008e00000000000000010000000a0024115553797374656d00120024115553797374656d2e44617461000000001a0024115553797374656d2e44617461\
2e53716c436c69656e7400001a0024115553797374656d2e446174612e53716c54797065730000001e002411554d6963726f736f66742e53716c5365727665722e53657276657200220024115553797374656d2e53656375726974792e43727970746f677261706879000000120024115553797374656d2e5465787400000000\
0e0024115553797374656d2e494f00002200201100000000010000110000000000000000696e707574427974654172726179000016000311380000007c010000770000000c0000000100000016002011010000000100001100000000000000006465730016002011020000000100001100000000000000006d73000002000600\
020006002e000404c93feac6b359d649bc250902bbabb460000000004d0044003200000004010000040000000c000000010008000200060032002a11000000009002000000000000710000000000000000000000020000068e0000000100000000456e63727970740000000016000311b80100005c020000710000008e000000\
0100000016000311ec01000058020000660000008e000000010000001600201100000000020000110000000000000000646573002200201101000000020000110000000000000000696e707574427974654172726179000002000600020006002e000404c93feac6b359d649bc250902bbabb460000000004d00440032000000\
04010000040100000c00000001000006020006002e002a1100000000f4020000000000002c000000000000000000000004000006ff00000001000000002e6363746f72002e000404c93feac6b359d649bc250902bbabb460000000004d0044003200000004010000040100000c0000000100000602000600f2000000b4000000\
00000000010001008e000000000000000d000000a8000000000000002e0000800c0000003100008027000000320000802d000000330000804300000034000080490000003500008060000000370000806b00000038000080700000003a000080750000003b000080830000003d000080840000003f0000808c00000042000080\
090035000d0049000d0036000d0050000d0029000d0061000d0040000d0022000d002f000d00350009000e000d00210005000600f20000009c0000008e0000000100010071000000000000000b0000009000000000000000530000801b0000005400008021000000550000802d00000056000080320000005700008049000000\
59000080540000005a000080590000005b000080660000005d000080670000005f0000806f000000610000800d0049000d0036000d0043000d0029000d0061000d0040000d0022000d00390009000e000d00210005000600f20000003c000000ff000000010001002c0000000000000003000000300000000000000011000080\
1600000016000080200000001b00008005005c000500370005002500f4000000080000000100000000000000180000003c000000540000006c000000840000009c000000b40000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
ffffffff1a092ff1400000002402000085000000010000006d000000010000005500000001000000b50000000100000025000000010000009d000000010000003d000000010000000100000001000000010000000000000000000000040000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000040000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000400000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000c0000001800000024000000300000003c0000004800000054000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
2200251100000000040000000200666e5f434c52456e4465637269707453534e000000001600291100000000040000000200303630303030303500001600251100000000040000000100446563727970740000001600291100000000040000000100303630303030303100001600251100000000b80100000100456e63727970\
740000001600291100000000b801000001003036303030303032000016002511000000009402000001002e6363746f720000000016002911000000009402000001003036303030303034000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
10000000000000000000000000000000000000000000000000000000ffffffff1a092ff10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
ffffffff77093101010000000c00108e0d0084690e000600b4000000740000002c0000005c000000000000000000000016000000190000000000eec00000000000000000ffff000000000000ffffffff00000000ffff0000000000000000000000000b00f802000000000000b401000001000000000000000000000000000000\
456e6372797074696f6e0032423643424133340000000000ffff000000000000ffffffff00000000ffff0000000000000000000000000a00b000000000000000900000000100000000000000000000000000000055736572446566696e656446756e6374696f6e730037314141414545380000002dba2ef10100000000000000\
8e00000000000000000000000000000000000000010000008e000000710000000000000000000000000000000000000001000000ff0000002c00000000000000000000000000000000000000010000002b0100004400000000000000010000000000000000000000020002000d01000000000100ffffffff000000006f010000\
0802000000000000ffffffff00000000ffffffff0200020000000100010001000000000000000000443a5c4964656e546563682d54696d65416964655765625c4465764f70735c5265706f5c54696d65416964652e434c525c434c5255736572446566696e656446756e632e63730000feeffeef010000000100000000010000\
000000000000000000ffffffffffffffffffff0900ffffffffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
942e3101ed412961010000004268e3020da2854789f4bd17ec24ebc0740000002f4c696e6b496e666f002f6e616d6573002f7372632f686561646572626c6f636b002f7372632f66696c65732f643a5c6964656e746563682d74696d65616964657765625c6465766f70735c7265706f5c74696d65616964652e636c725c636c\
7275736572646566696e656466756e632e6373000400000006000000010000003a0000000000000011000000070000000a000000060000000000000005000000220000000800000000000000dc513301000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0f00000020000000d0000000380000001f0200003800000000000000b4000000800000005c000000280000004c010000c8040000740200002c000000cc0000000300000016000000060000001400000015000000070000000a0000000b00000008000000090000000c0000000d0000000e0000000f0000001000000011000000\
1300000012000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
1700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000\
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000

AS 'TimeAide.CLR.pdb'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[fn_CLREnDecriptSSN]'
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
CREATE FUNCTION [dbo].[fn_CLREnDecriptSSN] (@ssnStr [nvarchar] (max), @actionType [nvarchar] (max))
RETURNS [nvarchar] (max)
WITH EXECUTE AS CALLER
EXTERNAL NAME [TimeAide.CLR].[UserDefinedFunctions].[fn_CLREnDecriptSSN]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END
GO


