USE [NS20211]

alter table NS_ADM_USER_R
add Token nvarchar(max)  NULL
alter table NS_ADM_USER_R
ADD TOKEN_EXP_DATE [datetime] NULL



USE [NS20211]

/****** Object:  Table [dbo].[tblErrorLog]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblErrorLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[DateTime] [datetime] NULL,
	[ErrorLocation] [text] NULL,
	[ErrorType] [text] NULL,
	[ErrorDescription] [text] NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_ErrorLog]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create PROCEDURE [dbo].[sp_ErrorLog] 
                         @Mode                          VARCHAR(10),
                         @Id                            INT                   = null,
                         @UserName                      VARCHAR(50)           = null,
                         @DateTime                      DATETIME              = null,
                         @ErrorLocation                 TEXT                  = null,
                         @ErrorType                     TEXT                  = null,
                         @ErrorDescription              TEXT                  = null
AS
BEGIN
     SET NOCOUNT ON;
          IF(@Mode='Select')
               BEGIN
                    SELECT 
                         Id,
                         UserName,
                         DateTime,
                         ErrorLocation,
                         ErrorType,
                         ErrorDescription
                     FROM tblErrorLog  WHERE 
                         Id= @Id;
               END
          ELSE IF(@Mode='Insert'and CONVERT(VARCHAR, @ErrorType)<>'HttpException')

               BEGIN
                    INSERT INTO tblErrorLog
                       (
                         UserName,
                         DateTime,
                         ErrorLocation,
                         ErrorType,
                         ErrorDescription
                       )
                       VALUES
                       (
                         @UserName,
                         @DateTime,
                         @ErrorLocation,
                         @ErrorType,
                         @ErrorDescription
                       )

               END
		  ELSE IF(@Mode='In')

               BEGIN
                    INSERT INTO tblErrorLog
                       (
                         UserName,
                         DateTime,
                         ErrorLocation,
                         ErrorType,
                         ErrorDescription
                       )
                       VALUES
                       (
                         @UserName,
                         @DateTime,
                         @ErrorLocation,
                         @ErrorType,
                         @ErrorDescription
                       )

               END
          ELSE IF(@Mode='Update')

               BEGIN
                    UPDATE tblErrorLog SET
                         UserName                      =  @UserName,
                         DateTime                      =  @DateTime,
                         ErrorLocation                 =  @ErrorLocation,
                         ErrorType                     =  @ErrorType,
                         ErrorDescription              =  @ErrorDescription
                     WHERE 
                         Id= @Id;
               END
          ELSE IF(@Mode='Delete')

               BEGIN
                    DELETE FROM tblErrorLog
                     WHERE 
                         Id= @Id;
               END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetMastersNS]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_GetMastersNS](        
@Mode Varchar(50)=null,  
@CompanyId int = null,
@LangId int = null,
@DocType int = null,
@BranchId int = null,
@ArTypeNo int = null)        
as        
Begin    
   
if(@Mode='Company')        
Begin         
select cmp_no as 'CompanyId',cmp_lname as 'CompanyLName',cmp_fname as 'CompanyFName',    
Gps,our_customer_id as 'OurCustomerId',up_u_id as 'Upuid',UP_TRMNL_NM as 'UpTrmnlname'    
from NS_ADM_S_CMPNY    
End       
else if(@Mode='SelectBranchId')        
Begin         
select brn_no as 'BranchId',cmp_no as 'CompanyId',grp_no as 'GroupId',brn_lname as 'BranchLName',  
brn_f_name as 'BranchFName',brn_year as 'BranchYear',local_currency as 'LocalCurrency',  
brn_l_address as 'BranchLAddress',brn_f_address as 'BranchFAddress',brn_l_not_1 as 'BranchLNot',  
tax_numbertype as 'TaxType',supplierstreetname as 'SupplierStreet', suppliercityname as 'SupplierCity',  
supplieridentificationcode as 'SupplierIdentificationCode',ad_u_id as 'Aduid',up_u_id as 'Upuid',  
ad_trmnl_nm as 'AdTrmnlname',up_trmnl_nm as 'UpTrmnlname'  
from NS_ADM_S_BRN where cmp_no=@CompanyId  
End    
else if(@Mode='SelectBranch')        
Begin         
select brn_no as 'BranchId',cmp_no as 'CompanyId',grp_no as 'GroupId',brn_lname as 'BranchLName',  
brn_f_name as 'BranchFName',brn_year as 'BranchYear',local_currency as 'LocalCurrency',  
brn_l_address as 'BranchLAddress',brn_f_address as 'BranchFAddress',brn_l_not_1 as 'BranchLNot',  
tax_numbertype as 'TaxType',supplierstreetname as 'SupplierStreet', suppliercityname as 'SupplierCity',  
supplieridentificationcode as 'SupplierIdentificationCode',ad_u_id as 'Aduid',up_u_id as 'Upuid',  
ad_trmnl_nm as 'AdTrmnlname',up_trmnl_nm as 'UpTrmnlname'  
from NS_ADM_S_BRN   
End 
else if(@Mode='GetCurrency')        
Begin         
select cur_no as 'CurrNo',CUR_NAME as 'CurrName',CUR_RATE as 'CurrRate',CUR_FRC_NO as 'CurrFracNo' from NS_GL_EX_RATE 
End 
else if(@Mode='GetWareHouse')        
Begin         
select W_CODE as 'WCode',W_NAME as 'WName',WHG_CODE as 'WHGCode',BRN_NO as 'BranchId' from NS_INV_WAREHOUSE_DETAILS  
End 
else if(@Mode='GetWareHousebyId')        
Begin         
select W_CODE as 'WCode',W_NAME as 'WName',WHG_CODE as 'WHGCode',BRN_NO as 'BranchId' from NS_INV_WAREHOUSE_DETAILS  
where BRN_NO=@BranchId
End 
else if(@Mode='GetBillType')        
Begin         
select LANG_NO as 'LangId',FLG_VALUE as 'FlagValue',FLG_CODE as 'FlagCode',
FLG_DESC as 'FlagDesc',DOC_TYPE as 'DocType'
from NS_ADM_S_FLAGS where lang_no=@LangId  and DOC_TYPE=@DocType 
End 
else if(@Mode='GetEmployee')        
Begin         
select emp_no as 'EmployeeId',emp_l_nm as 'EmployeeName',gndr as 'Gender',
e_mail as 'Email' , brn_no as 'BranchId' from NS_HR_S_EMP where brn_no=@BranchId
End 
else if(@Mode='GetCCode')        
Begin         
select v_code as ccode,v_a_name as cname ,v_brn_no as branchid from NS_AP_V_DETAILS where v_brn_no=@BranchId
union all
select c_code,c_a_name,c_brn_no from ns_ar_customer where c_brn_no=@BranchId
End 
else if(@Mode='GetSalesRep')        
Begin         
select sm_no,sm_name from NS_AR_SALES_MAN
End 
else if(@Mode='GetARType')        
Begin         
select AR_TYPE_NO,TYPE_NO,TYPE_NAME from NS_AR_TYPES_INPUT where AR_TYPE_NO=@ArTypeNo
End 
else if(@Mode='GetBillSource')        
Begin         
select * from NS_ADM_FORM_NAME where form_no in (158,160,130,133,155,126) and
LANG_NO=@LangId
End 
End
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUsers]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_GetUsers](        
@Mode Varchar(50)=null,        
@UserName Varchar(100)=null,        
@Password varchar(100)=null)        
as        
Begin        
if(@Mode='Select')        
Begin        
      
select NS_ADM_USER_R.u_id as 'UserID' ,NS_ADM_USER_R.USR_PASSWORD as 'Password' ,NS_ADM_USER_R.U_MNGR as 'UMngr' ,  
NS_ADM_USER_R.brn_no as 'BranchID', NS_ADM_USER_R.INACTIVE,  
NS_ADM_USER_R.GRP_NO as 'GroupNo',NS_ADM_USER_R.LGN_METHOD as 'LoginMethod',    
NS_ADM_S_BRN.BRN_NO,NS_ADM_S_CMPNY.CMP_NO as 'CompanyID','2021' as 'UnitYear',      
1 as 'UnitNumber',1 as 'LANG_NO', Token as 'Token' ,NS_ADM_USER_R.u_a_name as 'Name'    
from NS_ADM_USER_R  NS_ADM_USER_R      
left join NS_ADM_S_BRN NS_ADM_S_BRN on NS_ADM_S_BRN.GRP_NO=NS_ADM_USER_R.GRP_NO      
left join NS_ADM_S_CMPNY NS_ADM_S_CMPNY on NS_ADM_S_CMPNY.CMP_NO=NS_ADM_S_BRN.CMP_NO      
where NS_ADM_USER_R.U_ID=@UserName and NS_ADM_USER_R.USR_PASSWORD=@Password       
End        
End
GO
/****** Object:  StoredProcedure [dbo].[sp_InvoiceDetails]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InvoiceDetails] 
                        @Mode     VARCHAR(10),
						@BILL_SER numeric(27, 0)       = NULL,
						@BILL_DOC_TYPE int             = NULL,
						@BILL_NO numeric(27, 0)        = NULL,
						@BILL_DATE date                = NULL,
						@A_CY nvarchar(7)              = NULL,
						@BILL_RATE float               = NULL,
						@STOCK_RATE float              = NULL,
						@C_CODE nvarchar(30)           = NULL,
						@C_NAME nvarchar(100)          = NULL,
						@AC_DTL_TYP int                = NULL,
						@A_CODE nvarchar(30)           = NULL,
						@VAT_AMT float                 = NULL,
						@BILL_AMT       float          = NULL,
						@W_CODE int                    = NULL,
						@A_DESC nvarchar(1000)         = NULL,
						@BRN_NO int                    = NULL,
						@DISC_AMT float                = NULL,
						@DISC_AMT_MST float            = NULL,
						@DISC_AMT_DTL float            = NULL,
						@BILL_HUNG bit                 = NULL,
						@SOURC_BILL_NO numeric(27, 0)  = NULL,
						@SOURC_BILL_TYP int            = NULL,
						@PUSH_AMT float                = NULL,
						@RETURN_AMT float              = NULL,
						@BILL_CASH float               = NULL,
						@CASH_NO int                   = NULL,
						@BILL_BANK float               = NULL,
						@BANK_NO int                   = NULL,
						@BILL_DR_ACCOUNT float         = NULL,
						@BILL_RT_AMT float             = NULL,
						@PRNT_NO int                   = NULL,
						@OLD_DOC_SER numeric(27, 0)    = NULL,
						@AR_TYPE int                   = NULL,
						@PaymentDone bit               = NULL,
						@PaymentDate date              = NULL,
						@THE_DRIVER nvarchar(50)       = NULL,
						@AD_U_ID int                   = NULL,
						@AD_DATE datetime              = NULL,
						@UP_U_ID int                   = NULL,
						@UP_DATE datetime              = NULL,
						@AD_TRMNL_NM nvarchar(50)      = NULL,
						@UP_TRMNL_NM nvarchar(50)      = NULL,
						@BILL_GUID uniqueidentifier    = NULL,
						@BILL_COUNTER int              = NULL,
						@RoundingAmount float          = NULL,
						@C_PHONE nvarchar(max)         = NULL,
						@SAL_MAN int                   = NULL,
						@DUE_DATE date                 = NULL,
						@CC_CODE nvarchar(max)         = NULL,
						--item parameter---
						@Item_BILL_SER numeric(27, 0)  = NULL,
						@Item_BILL_DOC_TYPE int        = NULL,
						@Item_BILL_NO numeric(27, 0)   = NULL,
						@Item_I_CODE nvarchar(30)      = NULL,
						@Item_I_QTY float              = NULL,
						@Item_ITM_UNT nvarchar(100)    = NULL,
						@Item_P_SIZE float             = NULL,
						@Item_P_QTY float              = NULL,
						@Item_I_PRICE float            = NULL,
						@Item_STK_COST float           = NULL,
						@Item_W_CODE int               = NULL,
						@Item_EXPIRE_DATE date         = NULL,
						@Item_VAT_PER float            = NULL,
						@Item_VAT_AMT float            = NULL,
						@Item_RCRD_NO int              = NULL,
						@Item_DIS_PER float            = NULL,
						@Item_DIS_AMT float            = NULL,
						@Item_FREE_QTY float           = NULL,
						@Item_BARCODE nvarchar(100)    = NULL,
						@Item_AR_TYPE int              = NULL,
						@Item_BILL_GUID uniqueidentifier=NULL,
						--@Item_BILL_DTL_GUID uniqueidentifier=NULL,
						@Item_SAL_MAN int              = NULL,
						@Item_CC_CODE nvarchar(max)    = NULL,
						@Item_Field1 nvarchar(max)     = NULL,
						@Item_Field2 nvarchar(max)     = NULL,
						@Item_Field3 nvarchar(max)     = NULL,
						@Item_Field4 nvarchar(max)     = NULL,
						@Item_Field5 nvarchar(max)     = NULL,
						@Item_Field6 nvarchar(max)     = NULL,
						@Item_Field7 nvarchar(max)     = NULL,
						@Item_Field8 nvarchar(max)     = NULL,
						@Item_Field9 nvarchar(max)     = NULL,
						@Item_Field10 nvarchar(max)    = NULL,
						---end-----
						@LANG_NO        INT            = 2,
						@RecordFrom     INT            = NULL,
						@RecordTo       INT            = NULL,
						@OrderByValue   VARCHAR(100)   = 'BILL_NO',
						@OrderByType    VARCHAR(100)   = 'DESC'
AS
BEGIN
     SET NOCOUNT ON;

     DECLARE
          @SelectStatement      NVARCHAR(MAX),
          @ConditionStatement   NVARCHAR(MAX),
          @CountValue           VARCHAR(50),
          @SelectStatementCount NVARCHAR(MAX),
          @ParmDef              NVARCHAR(50)
          IF(@Mode='Select')
               BEGIN
                    SET @SelectStatement = N'WITH NS_AR_BILL_MSTTable AS '
                    SET @SelectStatement = CONCAT(@SelectStatement,'(SELECT ROW_NUMBER() OVER(ORDER BY NS_AR_BILL_MST.', @OrderByValue,' ',@OrderByType,')as RowNumber,@CountValue as TotalRowCount,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.BILL_SER,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.BILL_DOC_TYPE,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.BILL_NO,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.A_DESC,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.C_NAME,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.VAT_AMT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.BILL_AMT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_MST.DISC_AMT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_S_FLAGS.FLG_DESC')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_AR_BILL_MST ')
					SET @SelectStatement = CONCAT(@SelectStatement,'left join NS_ADM_S_FLAGS NS_ADM_S_FLAGS on NS_ADM_S_FLAGS.FLG_VALUE=NS_AR_BILL_MST.BILL_DOC_TYPE and DOC_TYPE=4 and  NS_ADM_S_FLAGS.LANG_NO=',@LANG_NO,' ')
                         IF(@BILL_SER IS NOT NULL) OR (LEN(@BILL_SER) > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_MST.BILL_SER=''',@BILL_SER,''' AND ')
                            END
						  IF(@BILL_GUID IS NOT NULL) OR (LEN(@BILL_GUID) > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_MST.BILL_GUID=''',@BILL_GUID,''' AND ')
                            END
                        IF(@C_NAME IS NOT NULL) OR (LEN(@C_NAME) > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_MST.C_NAME=''',@C_NAME,''' AND ')
                            END
					  IF(@BILL_DOC_TYPE IS NOT NULL AND @BILL_DOC_TYPE > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_MST.BILL_DOC_TYPE=',@BILL_DOC_TYPE,' AND ')
							END
                           IF(@BILL_NO IS NOT NULL) OR (LEN(@BILL_NO) > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_MST.BILL_NO=''',@BILL_NO,''' AND ')
							END
               IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' ) SELECT * FROM NS_AR_BILL_MSTTable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM NS_AR_BILL_MST';
                  END
               ELSE
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4),')  SELECT * FROM NS_AR_BILL_MSTTable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = CONCAT(N'SELECT @result = COUNT(1) FROM NS_AR_BILL_MST WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4))
                  END
				
               SET @ParmDef = N'@result varchar(30) OUTPUT';
               EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
               EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
               END
           IF(@Mode='Insert')

               BEGIN  
                    INSERT INTO NS_AR_BILL_MST
                       (
                        [BILL_SER] ,	[BILL_DOC_TYPE] ,	[BILL_NO] ,
						[BILL_DATE] ,	[A_CY] ,	[BILL_RATE] ,	[STOCK_RATE] ,
						[C_CODE] ,	[C_NAME] ,	[AC_DTL_TYP] ,	[A_CODE] ,
						[VAT_AMT] ,	[BILL_AMT] ,	[W_CODE] ,	[A_DESC] ,
						[BRN_NO] ,	[DISC_AMT] ,	[DISC_AMT_MST] ,	[DISC_AMT_DTL] ,
						[BILL_HUNG] ,	[SOURC_BILL_NO],	[SOURC_BILL_TYP] ,	[PUSH_AMT] ,
						[RETURN_AMT] ,	[BILL_CASH] ,	[CASH_NO] ,	[BILL_BANK] ,	[BANK_NO] ,
						[BILL_DR_ACCOUNT] ,	[BILL_RT_AMT] ,	[PRNT_NO] ,	[OLD_DOC_SER] ,
						[AR_TYPE] ,	[PaymentDone] ,	[PaymentDate] ,	[THE_DRIVER] ,
						[AD_U_ID] ,	[AD_DATE] ,	[UP_U_ID] ,	[UP_DATE],	[AD_TRMNL_NM] ,
						[UP_TRMNL_NM] ,	[BILL_GUID] ,	[BILL_COUNTER] ,	[RoundingAmount] ,
						[C_PHONE] ,	[SAL_MAN] ,	[DUE_DATE] ,	[CC_CODE] 
                       )
                       VALUES
                       (
                        @BILL_SER,	@BILL_DOC_TYPE,	@BILL_NO,	@BILL_DATE,
						@A_CY,	@BILL_RATE,	@STOCK_RATE,	@C_CODE,	@C_NAME,
						@AC_DTL_TYP,	@A_CODE,	@VAT_AMT,	@BILL_AMT,	@W_CODE,
						@A_DESC,	@BRN_NO,	@DISC_AMT,	@DISC_AMT_MST,	@DISC_AMT_DTL,
						@BILL_HUNG,	@SOURC_BILL_NO,	@SOURC_BILL_TYP,	@PUSH_AMT,	@RETURN_AMT,
						@BILL_CASH,	@CASH_NO,	@BILL_BANK,	@BANK_NO,	@BILL_DR_ACCOUNT,
						@BILL_RT_AMT,	@PRNT_NO,	@OLD_DOC_SER,	@AR_TYPE,	@PaymentDone,
						@PaymentDate,	@THE_DRIVER,	@AD_U_ID,	@AD_DATE,	@UP_U_ID,
						@UP_DATE,	@AD_TRMNL_NM,	@UP_TRMNL_NM,	@BILL_GUID,	@BILL_COUNTER,
						@RoundingAmount,	@C_PHONE,	@SAL_MAN,	@DUE_DATE,	@CC_CODE 
                       )
					   select @BILL_GUID

               END
           IF(@Mode='InsertItem')

               BEGIN
			   DECLARE @Item_BILL_DTL_GUID1 uniqueidentifier 
                SET @Item_BILL_DTL_GUID1 = NEWID()
                    INSERT INTO NS_AR_BILL_DTL
                       (
                        BILL_SER ,	BILL_DOC_TYPE ,	BILL_NO ,	I_CODE ,	I_QTY ,
						ITM_UNT ,	P_SIZE ,	P_QTY ,	I_PRICE ,	STK_COST ,
						W_CODE ,	EXPIRE_DATE,	VAT_PER ,	VAT_AMT ,	RCRD_NO ,
						DIS_PER ,	DIS_AMT ,	FREE_QTY ,	BARCODE ,	AR_TYPE ,
						BILL_GUID ,	BILL_DTL_GUID ,
						SAL_MAN ,	CC_CODE ,	Field1 ,
						Field2 ,	Field3 ,	Field4 ,	Field5 ,	Field6 ,
						Field7 ,	Field8 ,	Field9 ,	Field10 
                       )
                       VALUES
                       (
                        @Item_BILL_SER ,	@Item_BILL_DOC_TYPE ,	@Item_BILL_NO ,
						@Item_I_CODE ,	@Item_I_QTY ,	@Item_ITM_UNT ,	@Item_P_SIZE ,	@Item_P_QTY ,
						@Item_I_PRICE ,	@Item_STK_COST ,	@Item_W_CODE ,	@Item_EXPIRE_DATE,
						@Item_VAT_PER ,	@Item_VAT_AMT ,	@Item_RCRD_NO ,	@Item_DIS_PER ,	@Item_DIS_AMT ,
						@Item_FREE_QTY ,	@Item_BARCODE ,	@Item_AR_TYPE ,	@Item_BILL_GUID ,	@Item_BILL_DTL_GUID1 ,
						@Item_SAL_MAN ,	@Item_CC_CODE ,	@Item_Field1 ,	@Item_Field2 ,	@Item_Field3 ,
						@Item_Field4 ,	@Item_Field5 ,	@Item_Field6 ,	@Item_Field7 ,	@Item_Field8 ,
						@Item_Field9 ,	@Item_Field10 
                       )



               END
          IF(@Mode='GetInvoice')

               BEGIN  
                 select * from NS_AR_BILL_MST where BILL_GUID=@BILL_GUID

                 select * from  NS_AR_BILL_DTL where BILL_GUID=@BILL_GUID   

               END

END
GO
/****** Object:  StoredProcedure [dbo].[sp_PrivilegeDetails]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_PrivilegeDetails] 
                        @Mode     VARCHAR(10),
						@U_ID int                      = NULL,
					    @FORM_NO int                   = NULL,
						@RecordFrom     INT            = NULL,
						@RecordTo       INT            = NULL,
						@OrderByValue   VARCHAR(100)   = 'U_ID',
						@OrderByType    VARCHAR(100)   = 'DESC'
AS
BEGIN
     SET NOCOUNT ON;

     DECLARE
          @SelectStatement      NVARCHAR(MAX),
          @ConditionStatement   NVARCHAR(MAX),
          @CountValue           VARCHAR(50),
          @SelectStatementCount NVARCHAR(MAX),
          @ParmDef              NVARCHAR(50)
          IF(@Mode='Select')
               BEGIN
                    SET @SelectStatement = N'WITH NS_ADM_PRIVILEGETable AS '
                    SET @SelectStatement = CONCAT(@SelectStatement,'(SELECT ROW_NUMBER() OVER(ORDER BY NS_ADM_PRIVILEGE.', @OrderByValue,' ',@OrderByType,')as RowNumber,@CountValue as TotalRowCount,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.U_ID,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.FORM_NO,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.INCLUDE_FLAG,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.AD_FLAG,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.DEL_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.MOD_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.VIEW_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.PRINT_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.VWREP_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.VRFY_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.PST_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_ADM_PRIVILEGE.F_ORDER_NO,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.SCR_TYP,0) as SCR_TYP,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.PERIOD_CLOSE,0) as PERIOD_CLOSE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.PAYMENT_DONE,0) as PAYMENT_DONE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.AD_FLAG_FROM,0) as AD_FLAG_FROM,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.HUNG,0) as HUNG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.CONSTRAINT_REVIEW,0) as CONSTRAINT_REVIEW,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.BARCODE_PRINTING,0) as BARCODE_PRINTING,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.SAVE_FLAG,0) as SAVE_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.EXPORT_FLAG,0) as EXPORT_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.IMPORT_FLAG,0) as IMPORT_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.FIRST_FLAG,0) as FIRST_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.NEXT_FLAG,0) as NEXT_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.PREVIOUS_FLAG,0) as PREVIOUS_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.LAST_FLAG,0) as LAST_FLAG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'isnull(NS_ADM_PRIVILEGE.ARCHIVE_FLAG,0) as ARCHIVE_FLAG ')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_ADM_PRIVILEGE NS_ADM_PRIVILEGE ')
  
					  IF(@U_ID IS NOT NULL AND @U_ID > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_ADM_PRIVILEGE.U_ID=',@U_ID,' AND ')
							END
                            IF(@FORM_NO IS NOT NULL AND @FORM_NO > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_ADM_PRIVILEGE.FORM_NO=',@FORM_NO,' AND ')
							END
               IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' ) SELECT * FROM NS_ADM_PRIVILEGETable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM NS_ADM_PRIVILEGE';
                  END
               ELSE
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4),')  SELECT * FROM NS_ADM_PRIVILEGETable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = CONCAT(N'SELECT @result = COUNT(1) FROM NS_ADM_PRIVILEGE WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4))
                  END
               SET @ParmDef = N'@result varchar(30) OUTPUT';
               EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
               EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
               END
    

END
GO
/****** Object:  StoredProcedure [dbo].[sp_ProductDetails]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_ProductDetails](
                         @Mode varchar(50)              = NULL,
                         @I_CODE varchar(40)            = NULL,
						 @W_CODE         INT            = NULL,
	                     @RecordFrom     INT            = NULL,
						 @RecordTo       INT            = NULL,
						 @OrderByValue   VARCHAR(100)   = 'I_CODE',
						 @OrderByType    VARCHAR(100)   = 'DESC'
)  
as  
Begin  
  SET NOCOUNT ON;

     DECLARE
          @SelectStatement      NVARCHAR(MAX),
          @ConditionStatement   NVARCHAR(MAX),
          @CountValue           VARCHAR(50),
          @SelectStatementCount NVARCHAR(MAX),
          @ParmDef              NVARCHAR(50)


  IF(@Mode='GetProduct')
               BEGIN
                    SET @SelectStatement = N'WITH NS_INV_ITM_MSTTable AS '
                    SET @SelectStatement = CONCAT(@SelectStatement,'(SELECT ROW_NUMBER() OVER(ORDER BY M.', @OrderByValue,' ',@OrderByType,')as RowNumber,@CountValue as TotalRowCount,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'M.I_CODE,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'M.I_NAME,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'P.ITM_UNT,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'P.I_PRICE,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'W.AVL_QTY,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'W.W_CODE')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_INV_ITM_MST M ')
					SET @SelectStatement = CONCAT(@SelectStatement,'inner join NS_INV_ITEM_PRICE P on P.I_CODE=M.I_CODE ')
					SET @SelectStatement = CONCAT(@SelectStatement,'inner join NS_INV_ITM_WCODE W on W.I_CODE=P.I_CODE ')

                         IF(@I_CODE IS NOT NULL) OR (LEN(@I_CODE) > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'M.I_CODE like ''',@I_CODE,'%'' AND ')
                            END
                       
                       IF(@W_CODE IS NOT NULL AND @W_CODE > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'W.W_CODE=',@W_CODE,' AND ')
							END
					   IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
							BEGIN
								SET @SelectStatement = CONCAT(@SelectStatement,' ) SELECT * FROM NS_INV_ITM_MSTTable', 
														CASE WHEN @RecordFrom is null or @RecordTo is null 
															THEN 
																'' 
															ELSE 
																CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
															END)
								SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM NS_INV_ITM_MST';
							END
						ELSE
							BEGIN
								SET @SelectStatement = CONCAT(@SelectStatement,' WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4),
															')  SELECT * FROM NS_INV_ITM_MSTTable', 
															CASE WHEN @RecordFrom is null or @RecordTo is null 
																THEN 
																	'' 
																ELSE 
																	CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
																END)
								SET @SelectStatementCount = CONCAT(N'SELECT @result = COUNT(1) FROM NS_INV_ITM_MST M inner join NS_INV_ITEM_PRICE P on P.I_CODE=M.I_CODE 
								inner join NS_INV_ITM_WCODE W on W.I_CODE=P.I_CODE WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4))
							END
						SET @ParmDef = N'@result varchar(30) OUTPUT';

		
						EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
						EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
               END
  ELSE IF(@Mode='GetProductCode')
  BEGIN
                    SET @SelectStatement = N'WITH NS_INV_ITM_MSTTable AS '
                    SET @SelectStatement = CONCAT(@SelectStatement,'(SELECT ROW_NUMBER() OVER(ORDER BY MST.I_CODE)as RowNumber,@CountValue as TotalRowCount,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'MST.I_CODE,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'MST.I_NAME,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'MST.VAT_PER,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'MST.VAT_TYPE,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'DTL.ITM_UNT,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'DTL.P_SIZE')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_INV_ITM_MST MST ')
					SET @SelectStatement = CONCAT(@SelectStatement,'left join NS_INV_ITM_DTL DTL on DTL.I_CODE=MST.I_CODE ')

                         IF(@I_CODE IS NOT NULL) OR (LEN(@I_CODE) > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'MST.I_CODE=''',@I_CODE,''' AND ')
                            END
                       
                     
					   IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
							BEGIN
								SET @SelectStatement = CONCAT(@SelectStatement,' ) SELECT * FROM NS_INV_ITM_MSTTable', 
														CASE WHEN @RecordFrom is null or @RecordTo is null 
															THEN 
																'' 
															ELSE 
																CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
															END)
								SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM NS_INV_ITM_MST';
							END
						ELSE
							BEGIN
								SET @SelectStatement = CONCAT(@SelectStatement,' WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4),
															')  SELECT * FROM NS_INV_ITM_MSTTable', 
															CASE WHEN @RecordFrom is null or @RecordTo is null 
																THEN 
																	'' 
																ELSE 
																	CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
																END)
								SET @SelectStatementCount = CONCAT(N'SELECT @result = COUNT(1) FROM NS_INV_ITM_MST  MST left join NS_INV_ITM_DTL DTL on DTL.I_CODE=MST.I_CODE 
								WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4))
							END
						SET @ParmDef = N'@result varchar(30) OUTPUT';

		
						EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
						EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
  END
End
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateLoginToken]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_UpdateLoginToken]
(
@Mode nvarchar(50)=null,
@UserID int=null,        
@Token nvarchar(max)=null
)
AS
Begin
if(@mode='Update')
Begin
update NS_ADM_USER_R set token=@Token,TOKEN_EXP_DATE=dateadd(MINUTE, 30, getdate())  where U_ID=@UserID
End
if(@mode='Refresh')
Begin
update NS_ADM_USER_R set TOKEN_EXP_DATE=dateadd(MINUTE, 30, getdate())  where U_ID=@UserID
End
if(@mode='Select')
Begin
Select U_ID as 'UserID',token,TOKEN_EXP_DATE from NS_ADM_USER_R where U_ID=@UserID
End
End

GO
/****** Object:  StoredProcedure [dbo].[sp_VendorDetails]    Script Date: 11-02-2023 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_VendorDetails](
                         @Mode varchar(50)              = NULL,
                         @V_CODE varchar(40)            = NULL,
						 @V_A_NAME varchar(40)          = NULL,
						 @V_A_CODE varchar(40)          = NULL,
						 @CASH_TYPE_SAVE INT            = NULL,
	                     @RecordFrom     INT            = NULL,
						 @RecordTo       INT            = NULL
					
)  
as  
Begin  
  SET NOCOUNT ON;

     DECLARE
          @SelectStatement      NVARCHAR(MAX),
          @ConditionStatement   NVARCHAR(MAX),
		  @ConditionStatement1   NVARCHAR(MAX),
		  @ConditionStatement2   NVARCHAR(MAX),
          @CountValue           VARCHAR(50),
          @SelectStatementCount NVARCHAR(MAX),
          @ParmDef              NVARCHAR(50)


  IF(@Mode='GetVendor')
               BEGIN
			   
			   IF(@V_CODE IS NOT NULL or  @V_A_NAME IS NOT NULL or @V_A_CODE IS NOT NULL)
					  BEGIN
					  
			    IF(@V_CODE IS NOT NULL) OR (LEN(@V_CODE) > 0)
                            BEGIN
                            
							 SET @ConditionStatement = CONCAT(@ConditionStatement,' V_CODE = ''',@V_CODE,''' AND ')
							 SET @ConditionStatement1 = CONCAT(@ConditionStatement1,' C_CODE = ''',@V_CODE,''' AND ')
							 SET @ConditionStatement2 = CONCAT(@ConditionStatement2,' EMP_NO = ''',@V_CODE,''' AND ')
                            END
                       
                        IF(@V_A_NAME IS NOT NULL) OR (LEN(@V_A_NAME) > 0)
                            BEGIN
                          
							 SET @ConditionStatement = CONCAT(@ConditionStatement,' V_A_NAME = ''',@V_A_NAME,''' AND ')
							  SET @ConditionStatement1 = CONCAT(@ConditionStatement1,' C_A_NAME = ''',@V_A_NAME,''' AND ')
							   SET @ConditionStatement2 = CONCAT(@ConditionStatement2,' EMP_L_NM = ''',@V_A_NAME,''' AND ')
                            END
						  IF(@V_A_CODE IS NOT NULL) OR (LEN(@V_A_CODE) > 0)
                            BEGIN
                            
							 SET @ConditionStatement = CONCAT(@ConditionStatement,' V_A_CODE = ''',@V_A_CODE,''' AND ')
							  SET @ConditionStatement1 = CONCAT(@ConditionStatement1,' C_A_CODE = ''',@V_A_CODE,''' AND ')
							   SET @ConditionStatement2 = CONCAT(@ConditionStatement2,' A_CODE = ''',@V_A_CODE,''' AND ')
                            END
							
							IF(@ConditionStatement <> '') OR (LEN(@ConditionStatement) > 0) OR (@ConditionStatement IS not NULL)
							Begin
							set @ConditionStatement =CONCAT(Left(@ConditionStatement,LEN(@ConditionStatement)-4), '  ')
							set @ConditionStatement1 =CONCAT(Left(@ConditionStatement1,LEN(@ConditionStatement1)-4), '  ')
							set @ConditionStatement2 =CONCAT(Left(@ConditionStatement2,LEN(@ConditionStatement2)-4), '  ')
							
							End
                    SET @SelectStatement = N'WITH NS_AP_V_DETAILSTable AS ( SELECT * FROM ('
                    SET @SelectStatement = CONCAT(@SelectStatement,'SELECT ROW_NUMBER() OVER(ORDER BY V_CODE)as RowNumber,@CountValue as TotalRowCount, ')
					--SET @SelectStatement = CONCAT(@SelectStatement,'select ')
                    SET @SelectStatement = CONCAT(@SelectStatement,'V_CODE,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'V_A_NAME,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'V_A_CODE')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_AP_V_DETAILS  where 1=1 and ',@ConditionStatement)
					SET @SelectStatement = CONCAT(@SelectStatement,' union  all ')
					SET @SelectStatement = CONCAT(@SelectStatement,'select ROW_NUMBER() OVER(ORDER BY C_CODE) + (SELECT COUNT(*) FROM NS_AP_V_DETAILS  where 1=1 and ' +@ConditionStatement+' ) AS RowNumber,
					@CountValue as TotalRowCount,C_CODE as ''V_CODE'',C_A_NAME as ''V_A_NAME'',C_A_CODE as ''V_A_CODE'' from NS_AR_CUSTOMER C where 1=1 and  ',@ConditionStatement1)
					SET @SelectStatement = CONCAT(@SelectStatement,' union  all ')
					SET @SelectStatement = CONCAT(@SelectStatement,'select ROW_NUMBER() OVER(ORDER BY E.EMP_NO) + (SELECT COUNT(*) FROM NS_AP_V_DETAILS where 1=1 and ' +@ConditionStatement+')+
					(SELECT COUNT(*) FROM NS_AR_CUSTOMER C  where 1=1 and '+@ConditionStatement1+' ) AS RowNumber, 
					@CountValue as TotalRowCount,E.EMP_NO as ''V_CODE'',E.EMP_L_NM as ''V_A_NAME'',E.A_CODE as ''V_A_CODE'' from NS_HR_S_EMP E where 1=1 and  '+@ConditionStatement2+'  ) Z )  
					select * FROM NS_AP_V_DETAILSTable')

					
                        
					   IF(@ConditionStatement <> '') OR (LEN(@ConditionStatement) > 0) OR (@ConditionStatement IS not NULL)
							BEGIN
							
								SET @SelectStatement = CONCAT(@SelectStatement, '  ',  
															
															CASE WHEN @RecordFrom is null or @RecordTo is null 
																THEN 
																	'' 
																ELSE 
																	CONCAT(' where RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
																END)
								SET @SelectStatementCount =CONCAT( N'SELECT @result = COUNT(1) FROM(select V_CODE,V_A_NAME,V_A_CODE from  NS_AP_V_DETAILS where 1=1 and '+@ConditionStatement+' union all 
								select C_CODE as ''V_CODE'',C_A_NAME as ''V_A_NAME'',C_A_CODE as ''V_A_CODE''
								FROM NS_AR_CUSTOMER where 1=1 and '+@ConditionStatement1+' union all select EMP_NO as ''V_CODE'',EMP_L_NM as ''V_A_NAME'',A_CODE as ''V_A_CODE'' FROM NS_HR_S_EMP where 1=1 and  '+@ConditionStatement2+'',')a')
							
							END
					
						SET @ParmDef = N'@result varchar(30) OUTPUT';
						
						EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
						EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
               END
					ELSE
					BEGIN
					 SET @SelectStatement = N'WITH NS_AP_V_DETAILSTable AS ( SELECT * FROM ('
                    SET @SelectStatement = CONCAT(@SelectStatement,'SELECT ROW_NUMBER() OVER(ORDER BY V.V_CODE)as RowNumber,@CountValue as TotalRowCount, ')
					
                    SET @SelectStatement = CONCAT(@SelectStatement,'V.V_CODE,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'V.V_A_NAME,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'V.V_A_CODE')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_AP_V_DETAILS V ')
					SET @SelectStatement = CONCAT(@SelectStatement,' union  all ')
					SET @SelectStatement = CONCAT(@SelectStatement,'select ROW_NUMBER() OVER(ORDER BY C.C_CODE) + (SELECT COUNT(*) FROM NS_AP_V_DETAILS V) AS RowNumber,
					@CountValue as TotalRowCount,C.C_CODE as ''V_CODE'',C.C_A_NAME as ''V_A_NAME'',C.C_A_CODE as ''V_A_CODE'' from NS_AR_CUSTOMER C ')
					SET @SelectStatement = CONCAT(@SelectStatement,' union  all ')
					SET @SelectStatement = CONCAT(@SelectStatement,'select ROW_NUMBER() OVER(ORDER BY E.EMP_NO) + (SELECT COUNT(*) FROM NS_AP_V_DETAILS V)+ (SELECT COUNT(*) FROM NS_AR_CUSTOMER C) AS RowNumber, 
					@CountValue as TotalRowCount,E.EMP_NO as ''V_CODE'',E.EMP_L_NM as ''V_A_NAME'',E.A_CODE as ''V_A_CODE'' from NS_HR_S_EMP E ) Z )  ')

					
                         IF(@V_CODE IS NOT NULL) OR (LEN(@V_CODE) > 0)
                            BEGIN
                             -- SET @ConditionStatement = CONCAT(@ConditionStatement,'V_CODE like ''',@V_CODE,'%'' AND ')
							 SET @ConditionStatement = CONCAT(@ConditionStatement,'V_CODE = ''',@V_CODE,''' AND ')
                            END
                       
                        IF(@V_A_NAME IS NOT NULL) OR (LEN(@V_A_NAME) > 0)
                            BEGIN
                          
							 SET @ConditionStatement = CONCAT(@ConditionStatement,'V_A_NAME = ''',@V_A_NAME,''' AND ')
                            END
						  IF(@V_A_CODE IS NOT NULL) OR (LEN(@V_A_CODE) > 0)
                            BEGIN
                            
							 SET @ConditionStatement = CONCAT(@ConditionStatement,'V_A_CODE = ''',@V_A_CODE,''' AND ')
                            END
					   IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
							BEGIN
							
								SET @SelectStatement = CONCAT(@SelectStatement,' SELECT * FROM NS_AP_V_DETAILSTable', 
														CASE WHEN @RecordFrom is null or @RecordTo is null 
															THEN 
																'' 
															ELSE 
																CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
															END)
								SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM(select V_CODE from  NS_AP_V_DETAILS union all select C_CODE as ''V_CODE''
								FROM NS_AR_CUSTOMER union all select EMP_NO as ''V_CODE'' FROM NS_HR_S_EMP) as Count';
								
							END
						
						SET @ParmDef = N'@result varchar(30) OUTPUT';
						
						EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
						EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
					END
                   
               END
  ELSE IF(@Mode='CashBankNo')
               BEGIN
                    SET @SelectStatement = N'WITH NS_GL_CASH_IN_HAND_BANKTable AS '
                    SET @SelectStatement = CONCAT(@SelectStatement,'(SELECT ROW_NUMBER() OVER(ORDER BY CASH_NO )as RowNumber,@CountValue as TotalRowCount,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'CASH_NO,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'CASH_NAME')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_GL_CASH_IN_HAND_BANK ')
				

                      
                       
                       IF(@CASH_TYPE_SAVE IS NOT NULL AND @CASH_TYPE_SAVE > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'CASH_TYPE_SAVE=',@CASH_TYPE_SAVE,' AND ')
							END
					   IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
							BEGIN
								SET @SelectStatement = CONCAT(@SelectStatement,' ) SELECT * FROM NS_GL_CASH_IN_HAND_BANKTable', 
														CASE WHEN @RecordFrom is null or @RecordTo is null 
															THEN 
																'' 
															ELSE 
																CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
															END)
								SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM NS_GL_CASH_IN_HAND_BANK';
							END
						ELSE
							BEGIN
								SET @SelectStatement = CONCAT(@SelectStatement,' WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4),
															')  SELECT * FROM NS_GL_CASH_IN_HAND_BANKTable', 
															CASE WHEN @RecordFrom is null or @RecordTo is null 
																THEN 
																	'' 
																ELSE 
																	CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) 
																END)
								SET @SelectStatementCount = CONCAT(N'SELECT @result =COUNT(1) from NS_GL_CASH_IN_HAND_BANK WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4))
							END
						SET @ParmDef = N'@result varchar(30) OUTPUT';

		
						EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
						EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
               END
End
GO
