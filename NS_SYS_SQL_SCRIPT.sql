USE [NS_SYS]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetMasters]    Script Date: 11-02-2023 12:23:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER procedure [dbo].[sp_GetMasters](    
@Mode Varchar(50)=null)    
as    
Begin    
if(@Mode='Lang')    
Begin     
select lang_no as 'LangNo',LANG_NAME AS'LangName',REP_EXT AS 'RepExt',
LANG_EXT AS 'LangExt',LANG_DFLT AS 'LangDeft',LANG_DIR AS 'LangDir',FLG_ST AS 'FlgSet'
from [NS_SYS].[dbo].[NS_ADM_LANG_DEF]  
End   
else if(@Mode='AccountingYear')    
Begin     
select unit_no as 'UnitNo',unit_year as 'Unityear',unit_usr_f_nm as 'UnitUsrFname',unit_usr_l_nm as 'UnitUsrLname',
unit_usr_tst as 'UnitUsrTst',USE_ONLINE_FLG as 'Useonlineflg'
from [NS_SYS].[dbo].[NS_ADM_S_UNIT_USR]  
End 
End


   
GO


