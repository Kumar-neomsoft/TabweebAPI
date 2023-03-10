USE [NS20211]
GO
/****** Object:  StoredProcedure [dbo].[sp_PurchaseOrderDetails]    Script Date: 18-02-2023 21:04:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_PurchaseOrderDetails] 
                        @Mode     VARCHAR(10),
						@BILL_GUID uniqueidentifier    = NULL,
						@BILL_DOC_TYPE int             = NULL,
						@BILL_DATE date                = NULL,
						@A_CY nvarchar(7)              = NULL,
						@BILL_RATE float               = NULL,
						@STOCK_RATE float              = NULL,
						@V_CODE nvarchar(30)           = NULL,
						@V_NAME nvarchar(100)          = NULL,
						@AC_DTL_TYP int                = NULL,
						@A_CODE nvarchar(30)           = NULL,

						@W_CODE int                    = NULL,
						@A_DESC nvarchar(1000)         = NULL,
						@BRN_NO int                    = NULL,
		
						@BILL_HUNG bit                 = NULL,
						@SOURC_BILL_NO numeric(27, 0)  = NULL,
						@SOURC_BILL_TYP int            = NULL,
						@BILL_CASH float               = NULL,
						@CASH_NO int                   = NULL,
						@BILL_BANK float               = NULL,
						@BANK_NO int                   = NULL,
						@BILL_DR_ACCOUNT float         = NULL,
						@BILL_RT_AMT float             = NULL,
						@PRNT_NO int                   = NULL,
						@OLD_DOC_SER numeric(27, 0)    = NULL,
						@AR_TYPE int                   = NULL,
						@CERTIFIED int =NULL,
						@CERTIFIED_U_ID int = NULL,
						@CERTIFIED_DATE datetime= NULL,
						@CERTIFIED_NOTES nvarchar(max)= NULL,
						@CERTIFIED_USED int= NULL,
						@AD_U_ID int                   = NULL,
						@AD_DATE datetime              = NULL,
						@UP_U_ID int                   = NULL,
						@UP_DATE datetime              = NULL,
						@AD_TRMNL_NM nvarchar(50)      = NULL,
						@UP_TRMNL_NM nvarchar(50)      = NULL,
						@AP_TYPE int                   = NULL,
					
						--item parameter---
				
						@Item_I_CODE nvarchar(30)      = NULL,
						@Item_I_QTY float              = NULL,
						@Item_ITM_UNT nvarchar(100)    = NULL,
						@Item_P_SIZE float             = NULL,
						@Item_I_PRICE float            = NULL,
						@Item_STK_COST float           = NULL,
						@Item_W_CODE int               = NULL,
						@Item_VAT_PER float            = NULL,
						@Item_DIS_PER float            = NULL,
						@Item_DIS_AMT float            = NULL,
						@Item_FREE_QTY float           = NULL,
						@Item_BARCODE nvarchar(100)    = NULL,
						@Item_BILL_GUID uniqueidentifier=NULL,
						@Item_BILL_DTL_GUID uniqueidentifier=NULL,
						---end-----
						@PO_BILL_SER numeric(27, 0)  = NULL,
						@PO_BILL_NO INT            = NULL,
						-----------
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
                    SET @SelectStatement = N'WITH NS_AP_PI_BILL_ORDER_MSTTable AS '
                    SET @SelectStatement = CONCAT(@SelectStatement,'(SELECT ROW_NUMBER() OVER(ORDER BY NS_AP_PI_BILL_ORDER_MST.', @OrderByValue,' ',@OrderByType,')as RowNumber,@CountValue as TotalRowCount,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_SER,')
                    SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_DOC_TYPE,')
                   SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_NO,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_DATE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.A_CY, ')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_RATE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.STOCK_RATE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.V_CODE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.V_NAME,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.A_CODE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.VAT_AMT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_AMT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.DISC_AMT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.DISC_AMT_MST,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.DISC_AMT_DTL,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.AC_DTL_TYP,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.A_DESC,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_HUNG,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.SOURC_BILL_NO,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.SOURC_BILL_TYP,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_DR_ACCOUNT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.W_CODE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BRN_NO,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.AP_TYPE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_CASH,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.CASH_NO,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_BANK,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BANK_NO,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_RT_AMT,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.PRNT_NO,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.OLD_DOC_SER,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.AP_TYPE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.CERTIFIED,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.CERTIFIED_U_ID,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.CERTIFIED_DATE,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.CERTIFIED_NOTES,')
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.CERTIFIED_USED,')
				
					SET @SelectStatement = CONCAT(@SelectStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_GUID')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_AP_PI_BILL_ORDER_MST ')

                         IF(@BILL_GUID IS NOT NULL) OR (LEN(@BILL_GUID) > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_GUID=''',@BILL_GUID,''' AND ')
                            END
	
                        IF(@PO_BILL_SER IS NOT NULL) OR (LEN(@PO_BILL_SER) > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_SER=',@PO_BILL_SER,' AND ')
                            END
					  IF(@BILL_DOC_TYPE IS NOT NULL AND @BILL_DOC_TYPE > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_DOC_TYPE=',@BILL_DOC_TYPE,' AND ')
							END
                       IF(@PO_BILL_NO IS NOT NULL AND @PO_BILL_NO > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AP_PI_BILL_ORDER_MST.BILL_NO=',@PO_BILL_NO,' AND ')
							END
               IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' ) SELECT * FROM NS_AP_PI_BILL_ORDER_MSTTable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM NS_AP_PI_BILL_ORDER_MST';
                  END
               ELSE
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4),')  SELECT * FROM NS_AP_PI_BILL_ORDER_MSTTable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = CONCAT(N'SELECT @result = COUNT(1) FROM NS_AP_PI_BILL_ORDER_MST WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4))
                  END
				
               SET @ParmDef = N'@result varchar(30) OUTPUT';
			   print @SelectStatement
               EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
               EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
               END
          IF(@Mode='InsertPO')

               BEGIN  
			     DECLARE @BILL_NO numeric(27, 0)   = NULL
			     DECLARE @BILL_SER numeric(27, 0)  = NULL
				 SELECT @BILL_NO=max(BILL_NO)+1 from  NS_AP_PI_BILL_ORDER_MST 
				 IF(@AD_U_ID is null)
				 BEGIN
				 SET @AD_U_ID=1
				 END
				
				 SET @BILL_SER=CONCAT(YEAR(getdate()),@AD_U_ID,@BILL_NO)
			     DECLARE @BILL_ORDER_MST_GUID uniqueidentifier 
                 SET @BILL_ORDER_MST_GUID = NEWID()
                    INSERT INTO NS_AP_PI_BILL_ORDER_MST
                       (
						[BILL_SER] ,	[BILL_DOC_TYPE] ,	[BILL_NO] ,	[BILL_DATE] ,	[A_CY] ,	[BILL_RATE],	[STOCK_RATE] ,	[V_CODE] ,	[V_NAME] ,
						[AC_DTL_TYP] ,	[A_CODE] ,		
						[W_CODE] ,	[A_DESC] ,	[BRN_NO] ,	
						[BILL_HUNG] ,	[SOURC_BILL_NO] ,	[SOURC_BILL_TYP] ,	[BILL_CASH] ,	[CASH_NO] ,	[BILL_BANK] ,	[BANK_NO] ,
						[BILL_DR_ACCOUNT] ,	[BILL_RT_AMT] ,	[PRNT_NO] ,	[OLD_DOC_SER] ,	[CERTIFIED] ,	[CERTIFIED_U_ID] ,	[CERTIFIED_DATE] ,
						[CERTIFIED_NOTES] ,	[CERTIFIED_USED] ,	[AD_U_ID] ,	[AD_DATE] ,	[UP_U_ID] ,	[UP_DATE] ,	[AD_TRMNL_NM] ,	[UP_TRMNL_NM] ,
						[AP_TYPE] ,	[BILL_GUID] ,DISC_AMT_MST
                       )
                       VALUES
                       (
                        @BILL_SER,	@BILL_DOC_TYPE,	@BILL_NO,	@BILL_DATE,	@A_CY,	@BILL_RATE,	@STOCK_RATE,	@V_CODE,	@V_NAME,
						@AC_DTL_TYP,	@A_CODE,	
						@W_CODE,@A_DESC,	@BRN_NO,	
						@BILL_HUNG,	@SOURC_BILL_NO,	@SOURC_BILL_TYP,@BILL_CASH,	@CASH_NO,	@BILL_BANK,	@BANK_NO,	
						@BILL_DR_ACCOUNT,@BILL_RT_AMT,	@PRNT_NO,	@OLD_DOC_SER,@CERTIFIED,@CERTIFIED_U_ID,@CERTIFIED_DATE,
						@CERTIFIED_NOTES,@CERTIFIED_USED,@AD_U_ID,	@AD_DATE,	@UP_U_ID,@UP_DATE,@AD_TRMNL_NM,	@UP_TRMNL_NM,
						@AR_TYPE,@BILL_ORDER_MST_GUID,0		
								
                       )
					   select @BILL_ORDER_MST_GUID
					

               END
		  IF(@Mode='ItemInsert')
		  BEGIN
			
			   DECLARE @Item_BILL_SER numeric(27, 0)  = NULL
			   DECLARE @Item_BILL_DOC_TYPE int        = NULL
			   DECLARE @Item_BILL_NO numeric(27, 0)   = NULL
			   DECLARE @Item_P_QTY float              = NULL
			   DECLARE @Item_VAT_AMT float            = NULL
			   DECLARE @Item_AP_TYPE int              = NULL
			   DECLARE @Item_RCRD_NO int              = NULL
			   DECLARE @Chk_CASH_NO  int              = NULL
			   ------From Master--------
			   SELECT @Item_BILL_SER=BILL_SER from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_DOC_TYPE=BILL_DOC_TYPE from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_NO=BILL_NO from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_AP_TYPE=AP_TYPE from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   ------------------------
			   SELECT @Item_RCRD_NO=MAX(RCRD_NO) from  NS_AP_PI_BILL_ORDER_DTL where BILL_GUID=@Item_BILL_GUID
			    IF(@Item_RCRD_NO IS NULL)
				BEGIN
				SET @Item_RCRD_NO= 0
				END
				ELSE
				BEGIN
				SET @Item_RCRD_NO= @Item_RCRD_NO+1
				END
			   --------------------------
			    IF(@Item_I_QTY IS NOT NULL AND @Item_I_QTY > 0)
				BEGIN
				SET @Item_P_QTY= @Item_I_QTY * @Item_P_SIZE
				END
				IF(@Item_VAT_PER IS NOT NULL AND @Item_VAT_PER > 0)
				BEGIN
				SET @Item_VAT_AMT= (@Item_I_PRICE - @Item_DIS_AMT) * (@Item_VAT_PER/100)
				END

			   DECLARE @BILL_ORDER_DTL_GUID uniqueidentifier 
               SET @BILL_ORDER_DTL_GUID = NEWID()
                    INSERT INTO NS_AP_PI_BILL_ORDER_DTL
                       (
						[BILL_SER],	[BILL_DOC_TYPE] ,	[BILL_NO] ,	[I_CODE] ,	[I_QTY] ,	[ITM_UNT] ,	[P_SIZE] ,	[P_QTY] ,
						[I_PRICE] ,	[STK_COST] ,	[W_CODE] ,	[EXPIRE_DATE] ,	[VAT_PER] ,	[VAT_AMT] ,	[RCRD_NO] ,
						[DIS_PER] ,	[DIS_AMT] ,	[FREE_QTY] ,	[BARCODE] ,	[AP_TYPE] ,	[BILL_DTL_GUID] ,	[BILL_GUID] 
                       )
                       VALUES
                       (
                        @Item_BILL_SER ,	@Item_BILL_DOC_TYPE ,	@Item_BILL_NO ,	@Item_I_CODE ,	@Item_I_QTY ,	@Item_ITM_UNT ,	@Item_P_SIZE ,	@Item_P_QTY ,
						@Item_I_PRICE ,	@Item_STK_COST ,	@Item_W_CODE ,	GETDATE(),	@Item_VAT_PER ,	@Item_VAT_AMT ,	@Item_RCRD_NO ,	
						@Item_DIS_PER ,	@Item_DIS_AMT ,	@Item_FREE_QTY ,	@Item_BARCODE ,	@Item_AP_TYPE  ,	@BILL_ORDER_DTL_GUID ,@Item_BILL_GUID
                       )

					   -----------------------------Update Amount--------------------------------
					    DECLARE @VAT_AMT float                 = 0
						DECLARE @BILL_AMT float                = 0
						DECLARE @DISC_AMT float                = 0
						DECLARE @DISC_AMT_DTL float            = 0
						DECLARE @ITEM_ROW_AMT float            = 0
						DECLARE @CAL_I_QTY float               = 0
						DECLARE @CAL_I_PRICE float             = 0
						DECLARE @CAL_DISC_AMT float            = 0
						DECLARE @CalDisMst float               = 0
						DECLARE @CalDisDtl float               = 0
						DECLARE @CalCashAmt float               = 0
						DECLARE @CalBankAmt float               = 0
						DECLARE @ExisBankAmt float               = 0
						DECLARE @cnt INT = 1;
							
						WHILE @cnt <= (SELECT COUNT (BILL_SER) FROM NS_AP_PI_BILL_ORDER_DTL where BILL_GUID=@Item_BILL_GUID)
						BEGIN
						
						    WITH PurchaseOrder AS ( select * from(select ROW_NUMBER() OVER(order by BILL_SER asc) as  Rownumber,I_QTY,I_PRICE,DIS_AMT FROM NS_AP_PI_BILL_ORDER_DTL 
                            where BILL_GUID=@Item_BILL_GUID)z)select @CAL_I_QTY=I_QTY,@CAL_I_PRICE=I_PRICE,@CAL_DISC_AMT=DIS_AMT from PurchaseOrder 
							where Rownumber=@cnt
	
							SET @BILL_AMT = @BILL_AMT+(@CAL_I_QTY * @CAL_I_PRICE) 
							SET @DISC_AMT_DTL= @DISC_AMT_DTL +(@CAL_I_QTY * @CAL_DISC_AMT) 
						
							SELECT @CalDisMst=DISC_AMT_MST,@CalDisDtl=DISC_AMT_DTL,@Chk_CASH_NO=CASH_NO,@ExisBankAmt=BILL_BANK FROM  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
							IF(@CalDisMst is null)
							Begin
							set @CalDisMst=0
							End
							IF(@CalDisDtl is null)
							Begin
							set @CalDisDtl=0
							End

							IF(@Chk_CASH_NO is null)
							Begin
							set @Chk_CASH_NO=1
							End
						
							SET @DISC_AMT=@DISC_AMT + (@CalDisMst+@CalDisDtl)

							SET @VAT_AMT =((@BILL_AMT - @DISC_AMT_DTL) * 0.15)

							IF(@Chk_CASH_NO = 1)
							Begin
							set @CalCashAmt = ((@BILL_AMT+@VAT_AMT)-@DISC_AMT_DTL)
							SET @CalBankAmt=0
							End
							 IF(@Chk_CASH_NO = 2)
							BEGIN
							SET @CalCashAmt=((@BILL_AMT+@VAT_AMT)-@DISC_AMT_DTL)-@ExisBankAmt
							SET @CalBankAmt=@ExisBankAmt

							END

							
							UPDATE NS_AP_PI_BILL_ORDER_MST set BILL_AMT = @BILL_AMT,DISC_AMT_DTL=@DISC_AMT_DTL,DISC_AMT=@DISC_AMT_DTL ,VAT_AMT=@VAT_AMT,BILL_CASH=@CalCashAmt
							,BILL_BANK=@CalBankAmt
							WHERE  BILL_GUID=@Item_BILL_GUID;

							SET @cnt = @cnt + 1;
							
						END;

					 -----------------------------------------------------------------------------
	
               END
   
	       IF(@Mode='UpdatePO')

               BEGIN  
			   UPDATE NS_AP_PI_BILL_ORDER_MST SET BILL_DOC_TYPE=@BILL_DOC_TYPE,[BILL_DATE]=@BILL_DATE,[A_CY]=@A_CY,
			   [BILL_RATE]=@BILL_RATE,	[STOCK_RATE]=@STOCK_RATE,[V_CODE]=@V_CODE ,	[V_NAME]=@V_NAME,[AC_DTL_TYP]=@AC_DTL_TYP ,	[A_CODE]=@A_CODE ,
			   [W_CODE]=@W_CODE ,	[A_DESC]=@A_DESC ,	[BRN_NO]=@BRN_NO ,[BILL_HUNG]=@BILL_HUNG ,	[SOURC_BILL_NO]=@SOURC_BILL_NO ,
			   [SOURC_BILL_TYP]=@SOURC_BILL_TYP ,	[BILL_CASH]=@BILL_CASH ,	[CASH_NO]=@CASH_NO ,	[BILL_BANK]=@BILL_BANK ,	[BANK_NO]=@BANK_NO ,
			   [BILL_DR_ACCOUNT]=@BILL_DR_ACCOUNT ,	[BILL_RT_AMT]=@BILL_RT_AMT ,	[PRNT_NO]=@PRNT_NO ,	[OLD_DOC_SER]=@OLD_DOC_SER ,	
			   [CERTIFIED]=@CERTIFIED ,	[CERTIFIED_U_ID]=@CERTIFIED_U_ID ,	[CERTIFIED_DATE]=@CERTIFIED_DATE ,[CERTIFIED_NOTES]=@CERTIFIED_NOTES ,	
			   [CERTIFIED_USED]=@CERTIFIED_USED,[UP_U_ID]=@UP_U_ID ,	[UP_DATE]=@UP_DATE,[UP_TRMNL_NM]=@UP_TRMNL_NM,[AP_TYPE]=@AR_TYPE
			   WHERE BILL_GUID=@BILL_GUID
                
				
               END
			 IF(@Mode='ItemUpdate')

               BEGIN  
			   DECLARE @Item_BILL_SER_U numeric(27, 0)  = NULL
			   DECLARE @Item_BILL_DOC_TYPE_U int        = NULL
			   DECLARE @Item_BILL_NO_U numeric(27, 0)   = NULL
			   DECLARE @Item_P_QTY_U float              = NULL
			   DECLARE @Item_VAT_AMT_U float            = NULL
			   DECLARE @Item_AP_TYPE_U int              = NULL
			   DECLARE @Item_RCRD_NO_U int              = NULL
			   DECLARE @Chk_CASH_NO_U  int              = NULL
			   ------From Master--------
			   SELECT @Item_BILL_SER_U=BILL_SER from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_DOC_TYPE_U=BILL_DOC_TYPE from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_NO_U=BILL_NO from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_AP_TYPE_U=AP_TYPE from  NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@Item_BILL_GUID
			   ------------------------
			   SELECT @Item_RCRD_NO_U=MAX(RCRD_NO) from  NS_AP_PI_BILL_ORDER_DTL where BILL_GUID=@Item_BILL_GUID
			    IF(@Item_RCRD_NO_U IS NULL)
				BEGIN
				SET @Item_RCRD_NO_U= 0
				END
				ELSE
				BEGIN
				SET @Item_RCRD_NO_U= @Item_RCRD_NO_U+1
				END
			   --------------------------
			    IF(@Item_I_QTY IS NOT NULL AND @Item_I_QTY > 0)
				BEGIN
				SET @Item_P_QTY= @Item_I_QTY * @Item_P_SIZE
				END
				IF(@Item_VAT_PER IS NOT NULL AND @Item_VAT_PER > 0)
				BEGIN
				SET @Item_VAT_AMT= (@Item_I_PRICE - @Item_DIS_AMT) * (@Item_VAT_PER/100)
				END
				IF EXISTS(SELECT 1  FROM NS_AP_PI_BILL_ORDER_DTL WHERE BILL_DTL_GUID=@Item_BILL_DTL_GUID AND BILL_GUID=@Item_BILL_GUID)
				BEGIN
				UPDATE NS_AP_PI_BILL_ORDER_DTL SET [BILL_SER]=@Item_BILL_SER_U,[BILL_DOC_TYPE]=@Item_BILL_DOC_TYPE_U ,[BILL_NO]=@Item_BILL_NO_U,
				[I_CODE]=@Item_I_CODE ,	[I_QTY]=@Item_I_QTY ,	[ITM_UNT]=@Item_ITM_UNT ,	[P_SIZE]=@Item_P_SIZE ,	[P_QTY] =@Item_P_QTY,
				[I_PRICE]=@Item_I_PRICE ,	[STK_COST]=@Item_STK_COST ,	[W_CODE]=@Item_W_CODE ,	[EXPIRE_DATE]=GETDATE() ,	[VAT_PER]=@Item_VAT_PER ,	
				[VAT_AMT]=@Item_VAT_AMT ,[DIS_PER]=@Item_DIS_PER ,	[DIS_AMT]=@Item_DIS_AMT ,[FREE_QTY] =@Item_FREE_QTY,	[BARCODE]=@Item_BARCODE ,[AP_TYPE]=@Item_AP_TYPE
				WHERE BILL_DTL_GUID=@Item_BILL_DTL_GUID AND BILL_GUID=@Item_BILL_GUID
				END
				ELSE
				BEGIN
				 DECLARE @BILL_ORDER_DTL_GUID_U uniqueidentifier 
                 SET @BILL_ORDER_DTL_GUID_U = NEWID()
                       INSERT INTO NS_AP_PI_BILL_ORDER_DTL
                       (
						[BILL_SER],	[BILL_DOC_TYPE] ,	[BILL_NO] ,	[I_CODE] ,	[I_QTY] ,	[ITM_UNT] ,	[P_SIZE] ,	[P_QTY] ,
						[I_PRICE] ,	[STK_COST] ,	[W_CODE] ,	[EXPIRE_DATE] ,	[VAT_PER] ,	[VAT_AMT] ,	[RCRD_NO] ,
						[DIS_PER] ,	[DIS_AMT] ,	[FREE_QTY] ,	[BARCODE] ,	[AP_TYPE] ,	[BILL_DTL_GUID] ,	[BILL_GUID] 
                       )
                       VALUES
                       (
                        @Item_BILL_SER_U ,	@Item_BILL_DOC_TYPE_U ,	@Item_BILL_NO_U ,	@Item_I_CODE ,	@Item_I_QTY ,	@Item_ITM_UNT ,	@Item_P_SIZE ,	@Item_P_QTY ,
						@Item_I_PRICE ,	@Item_STK_COST ,	@Item_W_CODE ,	GETDATE(),	@Item_VAT_PER ,	@Item_VAT_AMT ,	@Item_RCRD_NO_U ,	
						@Item_DIS_PER ,	@Item_DIS_AMT ,	@Item_FREE_QTY ,	@Item_BARCODE ,	@Item_AP_TYPE  ,	@BILL_ORDER_DTL_GUID_U ,@Item_BILL_GUID
                       )
               END
					   -----------------------------Update Amount--------------------------------
					    DECLARE @VAT_AMT_U float                  = 0
						DECLARE @BILL_AMT_U  float                = 0
						DECLARE @DISC_AMT_U  float                = 0
					
						DECLARE @DISC_AMT_DTL_U  float            = 0
						DECLARE @ITEM_ROW_AMT_U  float            = 0
						DECLARE @CAL_I_QTY_U  float               = 0
						DECLARE @CAL_I_PRICE_U  float             = 0
						DECLARE @CAL_DISC_AMT_U  float            = 0
						DECLARE @CalDisMst_U  float               = 0
						DECLARE @CalDisDtl_U  float               = 0
						DECLARE @CalCashAmt_U float               = 0
						DECLARE @CalBankAmt_U float               = 0
						DECLARE @ExisBankAmt_U float               = 0
						DECLARE @cnt_U  INT = 1;

						WHILE @cnt_U  <= (SELECT COUNT (BILL_SER) FROM NS_AP_PI_BILL_ORDER_DTL where BILL_GUID=@Item_BILL_GUID)
						BEGIN
						    WITH PurchaseOrder AS ( select * from(select ROW_NUMBER() OVER(order by BILL_SER) as  Rownumber,I_QTY,I_PRICE,DIS_AMT FROM NS_AP_PI_BILL_ORDER_DTL 
                            where BILL_GUID=@Item_BILL_GUID)z)select @CAL_I_QTY_U =I_QTY,@CAL_I_PRICE_U =I_PRICE,@CAL_DISC_AMT_U =DIS_AMT from PurchaseOrder 
							where Rownumber=@cnt_U 
							SET @BILL_AMT_U  = @BILL_AMT_U +(@CAL_I_QTY_U  * @CAL_I_PRICE_U ) 
							SET @DISC_AMT_DTL_U = @DISC_AMT_DTL_U  +(@CAL_I_QTY_U  * @CAL_DISC_AMT_U ) 

							SELECT @CalDisMst_U =DISC_AMT_MST,@CalDisDtl_U =DISC_AMT_DTL,@Chk_CASH_NO_U=CASH_NO,@ExisBankAmt_U=BILL_BANK  FROM  NS_AP_PI_BILL_ORDER_MST 
							where BILL_GUID=@Item_BILL_GUID
							IF(@CalDisMst_U  is null)
							Begin
							set @CalDisMst_U =0
							End
							IF(@CalDisDtl_U  is null)
							Begin
							set @CalDisDtl_U =0
							End
							IF(@Chk_CASH_NO is null)
							Begin
							set @Chk_CASH_NO=1
							End

							SET @DISC_AMT_U =@DISC_AMT_U  + (@CalDisMst_U +@CalDisDtl_U )

							SET @VAT_AMT_U  =((@BILL_AMT_U  - @DISC_AMT_DTL_U ) * 0.15)

							IF(@Chk_CASH_NO = 1)
							Begin
							set @CalCashAmt_U = ((@BILL_AMT_U+@VAT_AMT_U)-@DISC_AMT_DTL_U)
							SET @CalBankAmt_U=0
							End
							 IF(@Chk_CASH_NO = 2)
							BEGIN
							SET @CalCashAmt_U=((@BILL_AMT_U+@VAT_AMT_U)-@DISC_AMT_DTL_U)-@ExisBankAmt_U
							SET @CalBankAmt_U=@ExisBankAmt_U

							END
							
							UPDATE NS_AP_PI_BILL_ORDER_MST set BILL_AMT = @BILL_AMT_U ,DISC_AMT_DTL=@DISC_AMT_DTL_U ,DISC_AMT=@DISC_AMT_DTL_U  ,VAT_AMT=@VAT_AMT_U 
							,BILL_CASH=@CalCashAmt_U,BILL_BANK=@CalBankAmt_U 
							WHERE  BILL_GUID=@Item_BILL_GUID;

							SET @cnt_U  = @cnt_U  + 1;
							
						END;

					 ----------------------------------------------------------------------------- 
				
               END
          IF(@Mode='POEdit')

               BEGIN  
                 select * from NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@BILL_GUID

                 select * from  NS_AP_PI_BILL_ORDER_DTL where BILL_GUID=@BILL_GUID   
				
               END
          IF(@Mode='DeletePO')
               BEGIN  
			     Delete from  NS_AP_PI_BILL_ORDER_DTL where BILL_GUID=@BILL_GUID 
                 Delete from NS_AP_PI_BILL_ORDER_MST where BILL_GUID=@BILL_GUID
				 select 0
               END
		 IF(@Mode='ItemDetail')

               BEGIN  
			   --print 1
                 select * from  NS_AP_PI_BILL_ORDER_DTL where BILL_GUID=@BILL_GUID   
				
               END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_QuotationDetails]    Script Date: 18-02-2023 21:04:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_QuotationDetails] 
                        @Mode     VARCHAR(10),
						@BILL_GUID uniqueidentifier    = NULL,
						@BILL_DOC_TYPE int             = NULL,
						@BILL_DATE date                = NULL,
						@A_CY nvarchar(7)              = NULL,
						@BILL_RATE float               = NULL,
						@STOCK_RATE float              = NULL,
						@C_CODE nvarchar(30)           = NULL,
						@C_NAME nvarchar(100)          = NULL,
						@AC_DTL_TYP int                = NULL,
						@A_CODE nvarchar(30)           = NULL,
						@W_CODE int                    = NULL,
						@A_DESC nvarchar(1000)         = NULL,
						@BRN_NO int                    = NULL,
						@BILL_HUNG bit                 = NULL,
						@SOURC_BILL_NO numeric(27, 0)  = NULL,
						@SOURC_BILL_TYP int            = NULL,
						@BILL_CASH float               = NULL,
						@CASH_NO int                   = NULL,
						@BILL_BANK float               = NULL,
						@BANK_NO int                   = NULL,
						@BILL_DR_ACCOUNT float         = NULL,
						@BILL_RT_AMT float             = NULL,
						@PRNT_NO int                   = NULL,
						@OLD_DOC_SER numeric(27, 0)    = NULL,
						@AR_TYPE int                   = NULL,
						@CERTIFIED int =NULL,
						@CERTIFIED_U_ID int = NULL,
						@CERTIFIED_DATE datetime= NULL,
						@CERTIFIED_NOTES nvarchar(max)= NULL,
						@CERTIFIED_USED int= NULL,
						@AD_U_ID int                   = NULL,
						@AD_DATE datetime              = NULL,
						@UP_U_ID int                   = NULL,
						@UP_DATE datetime              = NULL,
						@AD_TRMNL_NM nvarchar(50)      = NULL,
						@UP_TRMNL_NM nvarchar(50)      = NULL,
						@SAL_MAN int                   = NULL,
					
						--item parameter---
	
						@Item_I_CODE nvarchar(30)      = NULL,
						@Item_I_QTY float              = NULL,
						@Item_ITM_UNT nvarchar(100)    = NULL,
						@Item_P_SIZE float             = NULL,
						@Item_I_PRICE float            = NULL,
						@Item_STK_COST float           = NULL,
						@Item_W_CODE int               = NULL,
						@Item_VAT_PER float            = NULL,
						@Item_DIS_PER float            = NULL,
						@Item_DIS_AMT float            = NULL,
						@Item_FREE_QTY float           = NULL,
						@Item_BARCODE nvarchar(100)    = NULL,
						@Item_SAL_MAN int              = NULL,
						@Item_BILL_GUID uniqueidentifier=NULL,
						@Item_BILL_DTL_GUID uniqueidentifier=NULL,
						@Item_Field1 nvarchar(100)    = NULL,
						@Item_Field2 nvarchar(100)    = NULL,
						@Item_Field3 nvarchar(100)    = NULL,
						@Item_Field4 nvarchar(100)    = NULL,
						@Item_Field5 nvarchar(100)    = NULL,
						@Item_Field6 nvarchar(100)    = NULL,
						@Item_Field7 nvarchar(100)    = NULL,
						@Item_Field8 nvarchar(100)    = NULL,
						@Item_Field9 nvarchar(100)    = NULL,
						@Item_Field10 nvarchar(100)    = NULL,
						---end-----
						@PO_BILL_SER numeric(27, 0)  = NULL,
						@PO_BILL_NO INT            = NULL,
						-----------
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
			
                    SET @SelectStatement = N'WITH NS_AR_BILL_SHOW_PRICE_MSTTable AS '
                    SET @SelectStatement = CONCAT(@SelectStatement,'(SELECT ROW_NUMBER() OVER(ORDER BY NS_AR_BILL_SHOW_PRICE_MST.', @OrderByValue,' ',@OrderByType,')as RowNumber,@CountValue as TotalRowCount,')
     --               SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_SER,')
	                SET @SelectStatement = CONCAT(@SelectStatement,' * ')
     --               SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_DOC_TYPE,')
     --              SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_NO,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_DATE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.A_CY, ')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_RATE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.STOCK_RATE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.C_CODE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.C_NAME,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.A_CODE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.AC_DTL_TYP,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.VAT_AMT,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_AMT,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.W_CODE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.A_DESC,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BRN_NO,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.DISC_AMT,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.DISC_AMT_MST,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.DISC_AMT_DTL,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_HUNG,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.SOURC_BILL_NO,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.SOURC_BILL_TYP,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_DR_ACCOUNT,')
					----SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.AP_TYPE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.PUSH_AMT,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.RETURN_AMT,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_CASH,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.CASH_NO,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_BANK,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BANK_NO,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_RT_AMT,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.PRNT_NO,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.OLD_DOC_SER,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.AR_TYPE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.CERTIFIED,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.CERTIFIED_U_ID,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.CERTIFIED_DATE,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.CERTIFIED_NOTES,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.CERTIFIED_USED,')
				 --   SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_GUID,')
					--SET @SelectStatement = CONCAT(@SelectStatement,'NS_AR_BILL_SHOW_PRICE_MST.SAL_MAN ')
                    SET @SelectStatement = CONCAT(@SelectStatement,' FROM NS_AR_BILL_SHOW_PRICE_MST  ')
					
                         IF(@BILL_GUID IS NOT NULL and LEN(@BILL_GUID) > 0 )
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_GUID=''',@BILL_GUID,''' AND ')
                            END
                        IF(@PO_BILL_SER IS NOT NULL AND @PO_BILL_SER > 0)
                            BEGIN
                              SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_SER=',@PO_BILL_SER,' AND ')
                            END
					  IF(@BILL_DOC_TYPE IS NOT NULL AND @BILL_DOC_TYPE > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_DOC_TYPE=',@BILL_DOC_TYPE,' AND ')
							END
                       IF(@PO_BILL_NO IS NOT NULL AND @PO_BILL_NO > 0)
							BEGIN
								SET @ConditionStatement = CONCAT(@ConditionStatement,'NS_AR_BILL_SHOW_PRICE_MST.BILL_NO=',@PO_BILL_NO,' AND ')
							END
							print 3
               IF(@ConditionStatement = '') OR (LEN(@ConditionStatement) = 0) OR (@ConditionStatement IS NULL)
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' ) SELECT * FROM NS_AR_BILL_SHOW_PRICE_MSTTable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = N'SELECT @result = COUNT(1) FROM NS_AR_BILL_SHOW_PRICE_MST';
                  END
               ELSE
                  BEGIN
                    SET @SelectStatement = CONCAT(@SelectStatement,' WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4),')  SELECT * FROM NS_AR_BILL_SHOW_PRICE_MSTTable', CASE WHEN @RecordFrom is null or @RecordTo is null THEN '' ELSE CONCAT(' WHERE RowNumber BETWEEN ', @RecordFrom,' AND ', @RecordTo) END)
                    SET @SelectStatementCount = CONCAT(N'SELECT @result = COUNT(1) FROM NS_AR_BILL_SHOW_PRICE_MST WHERE ',Left(@ConditionStatement,LEN(@ConditionStatement)-4))
                  END
				
               SET @ParmDef = N'@result varchar(30) OUTPUT';
			   print @SelectStatement
               EXECUTE  SP_EXECUTESQL @SelectStatementCount, @ParmDef, @result=@CountValue OUTPUT;
               EXECUTE  SP_EXECUTESQL @SelectStatement,N'@CountValue varchar(10)',@CountValue;
               END
          IF(@Mode='InsertQuo')

               BEGIN  
			   
			     DECLARE @BILL_NO numeric(27, 0)   = NULL
			     DECLARE @BILL_SER numeric(27, 0)  = NULL
				 SELECT @BILL_NO=max(BILL_NO)+1 from  NS_AR_BILL_SHOW_PRICE_MST 
				 IF(@AD_U_ID is null)
				 BEGIN
				 SET @AD_U_ID=1
				 END

				 SET @BILL_SER=CONCAT(YEAR(getdate()),@BRN_NO,@AD_U_ID,@BILL_NO)
			     DECLARE @BILL_SHOW_PRICE_MST_GUID uniqueidentifier 
                 SET @BILL_SHOW_PRICE_MST_GUID = NEWID()
			
                    INSERT INTO NS_AR_BILL_SHOW_PRICE_MST
                       (
						[BILL_SER] ,[BILL_DOC_TYPE],[BILL_NO],[BILL_DATE],[A_CY] ,[BILL_RATE],[STOCK_RATE] ,[C_CODE] ,[C_NAME] ,
						[AC_DTL_TYP] ,[A_CODE] ,[W_CODE] ,[A_DESC] ,[BRN_NO] ,[BILL_HUNG] ,	[SOURC_BILL_NO] ,[SOURC_BILL_TYP] ,	
						[BILL_CASH] ,	[CASH_NO] ,	[BILL_BANK] ,	[BANK_NO] ,
						[BILL_DR_ACCOUNT] ,	[BILL_RT_AMT] ,	[PRNT_NO] ,	[OLD_DOC_SER] ,	[CERTIFIED] ,	[CERTIFIED_U_ID] ,	[CERTIFIED_DATE] ,
						[CERTIFIED_NOTES] ,	[CERTIFIED_USED] ,	[AD_U_ID] ,	[AD_DATE] ,	[UP_U_ID] ,	[UP_DATE] ,	[AD_TRMNL_NM] ,	[UP_TRMNL_NM] ,
						[SAL_MAN] ,	[BILL_GUID] ,DISC_AMT_MST
                       )
                       VALUES
                       (
                        @BILL_SER,	@BILL_DOC_TYPE,	@BILL_NO,	@BILL_DATE,	@A_CY,	@BILL_RATE,	@STOCK_RATE,@C_CODE,@C_NAME,
						@AC_DTL_TYP,	@A_CODE,@W_CODE,@A_DESC,	@BRN_NO,@BILL_HUNG,	@SOURC_BILL_NO,	@SOURC_BILL_TYP,
						@BILL_CASH,	@CASH_NO,	@BILL_BANK,	@BANK_NO,	
						@BILL_DR_ACCOUNT,@BILL_RT_AMT,	@PRNT_NO,	@OLD_DOC_SER,@CERTIFIED,@CERTIFIED_U_ID,@CERTIFIED_DATE,
						@CERTIFIED_NOTES,@CERTIFIED_USED,@AD_U_ID,	@AD_DATE,	@UP_U_ID,@UP_DATE,@AD_TRMNL_NM,	@UP_TRMNL_NM,
						@SAL_MAN,@BILL_SHOW_PRICE_MST_GUID,0		
								
                       )
					   select @BILL_SHOW_PRICE_MST_GUID
					

               END
		  IF(@Mode='InsertItem')
		  BEGIN
			
			   DECLARE @Item_BILL_SER numeric(27, 0)  = NULL
			   DECLARE @Item_BILL_DOC_TYPE int        = NULL
			   DECLARE @Item_BILL_NO numeric(27, 0)   = NULL
			   DECLARE @Item_P_QTY float              = NULL
			   DECLARE @Item_VAT_AMT float            = NULL
			   DECLARE @Item_AR_TYPE int              = NULL
			   DECLARE @Item_RCRD_NO int              = NULL
			   DECLARE @Chk_CASH_NO  int              = NULL
			   ------From Master--------
			   SELECT @Item_BILL_SER=BILL_SER from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_DOC_TYPE=BILL_DOC_TYPE from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_NO=BILL_NO from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_AR_TYPE=AR_TYPE from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			   ------------------------
			   SELECT @Item_RCRD_NO=MAX(RCRD_NO) from  NS_AR_BILL_SHOW_PRICE_DLT where BILL_GUID=@Item_BILL_GUID
			    IF(@Item_RCRD_NO IS NULL)
				BEGIN
				SET @Item_RCRD_NO= 0
				END
				ELSE
				BEGIN
				SET @Item_RCRD_NO= @Item_RCRD_NO+1
				END
			   --------------------------
			    IF(@Item_I_QTY IS NOT NULL AND @Item_I_QTY > 0)
				BEGIN
				SET @Item_P_QTY= @Item_I_QTY * @Item_P_SIZE
				END
				IF(@Item_VAT_PER IS NOT NULL AND @Item_VAT_PER > 0)
				BEGIN
				SET @Item_VAT_AMT= (@Item_I_PRICE - @Item_DIS_AMT) * (@Item_VAT_PER/100)
				END

			   DECLARE @BILL_SHOW_PRICE_DLT_GUID uniqueidentifier 
               SET @BILL_SHOW_PRICE_DLT_GUID = NEWID()
			
                    INSERT INTO NS_AR_BILL_SHOW_PRICE_DLT
                       (
						[BILL_SER],	[BILL_DOC_TYPE] ,	[BILL_NO] ,	[I_CODE] ,	[I_QTY] ,	[ITM_UNT] ,	[P_SIZE] ,	[P_QTY] ,
						[I_PRICE] ,	[STK_COST] ,	[W_CODE] ,	[EXPIRE_DATE] ,	[VAT_PER] ,	[VAT_AMT] ,	[RCRD_NO] ,
						[DIS_PER] ,	[DIS_AMT] ,	[FREE_QTY] ,	[BARCODE] ,	[AR_TYPE] ,	[BILL_DTL_GUID] ,	[BILL_GUID],Field1,Field2,
						Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10
                       )
                       VALUES
                       (
                        @Item_BILL_SER ,	@Item_BILL_DOC_TYPE ,	@Item_BILL_NO ,	@Item_I_CODE ,	@Item_I_QTY ,	@Item_ITM_UNT ,	@Item_P_SIZE ,	@Item_P_QTY ,
						@Item_I_PRICE ,	@Item_STK_COST ,	@Item_W_CODE ,	GETDATE(),	@Item_VAT_PER ,	@Item_VAT_AMT ,	@Item_RCRD_NO ,	
						@Item_DIS_PER ,	@Item_DIS_AMT ,	@Item_FREE_QTY ,	@Item_BARCODE ,	@Item_AR_TYPE  ,	@BILL_SHOW_PRICE_DLT_GUID ,@Item_BILL_GUID,
						@Item_Field1,@Item_Field2,@Item_Field3,@Item_Field4,@Item_Field5,@Item_Field6,@Item_Field7,@Item_Field8,@Item_Field9,@Item_Field10
                       )

					   -----------------------------Update Amount--------------------------------
					    DECLARE @VAT_AMT decimal(18,2)                 = 0
						DECLARE @BILL_AMT decimal(18,2)                 = 0
						DECLARE @DISC_AMT float                = 0
						DECLARE @DISC_AMT_DTL float            = 0
						DECLARE @ITEM_ROW_AMT float            = 0
						DECLARE @CAL_I_QTY float               = 0
						DECLARE @CAL_I_PRICE float             = 0
						DECLARE @CAL_DISC_AMT float            = 0
						DECLARE @CalDisMst float               = 0
						DECLARE @CalDisDtl float               = 0
					
						DECLARE @cnt INT = 1;
							
						WHILE @cnt <= (SELECT COUNT (BILL_SER) FROM NS_AR_BILL_SHOW_PRICE_DLT where BILL_GUID=@Item_BILL_GUID)
						BEGIN
						
						    WITH Quotation AS ( select * from(select ROW_NUMBER() OVER(order by BILL_SER asc) as  Rownumber,I_QTY,I_PRICE,DIS_AMT FROM NS_AR_BILL_SHOW_PRICE_DLT 
                            where BILL_GUID=@Item_BILL_GUID)z)select @CAL_I_QTY=I_QTY,@CAL_I_PRICE=I_PRICE,@CAL_DISC_AMT=DIS_AMT from Quotation 
							where Rownumber=@cnt
	
							SET @BILL_AMT = @BILL_AMT+(@CAL_I_QTY * @CAL_I_PRICE) 
							SET @DISC_AMT_DTL= @DISC_AMT_DTL +(@CAL_I_QTY * @CAL_DISC_AMT) 
						
							SELECT @CalDisMst=DISC_AMT_MST,@CalDisDtl=DISC_AMT_DTL,@Chk_CASH_NO=CASH_NO--,@ExisBankAmt=BILL_BANK 
							FROM  NS_AR_BILL_SHOW_PRICE_MST 
							where BILL_GUID=@Item_BILL_GUID
							IF(@CalDisMst is null)
							Begin
							set @CalDisMst=0
							End
							IF(@CalDisDtl is null)
							Begin
							set @CalDisDtl=0
							End

							IF(@Chk_CASH_NO is null)
							Begin
							set @Chk_CASH_NO=1
							End
						
							SET @DISC_AMT=@DISC_AMT + (@CalDisMst+@CalDisDtl)

							SET @VAT_AMT =((@BILL_AMT - @DISC_AMT_DTL) * 0.15)

							UPDATE NS_AR_BILL_SHOW_PRICE_MST set BILL_AMT = @BILL_AMT,DISC_AMT_DTL=@DISC_AMT_DTL,DISC_AMT=@DISC_AMT_DTL ,VAT_AMT=@VAT_AMT
			
							WHERE  BILL_GUID=@Item_BILL_GUID;

							SET @cnt = @cnt + 1;
							
						END;

					 -----------------------------------------------------------------------------
	
               END
   
	       IF(@Mode='UpdateQuo')

               BEGIN  
			   
			   UPDATE NS_AR_BILL_SHOW_PRICE_MST SET BILL_DOC_TYPE=@BILL_DOC_TYPE,[BILL_DATE]=@BILL_DATE,[A_CY]=@A_CY,
			   [BILL_RATE]=@BILL_RATE,	[STOCK_RATE]=@STOCK_RATE,[C_CODE]=@C_CODE ,	[C_NAME]=@C_NAME,[AC_DTL_TYP]=@AC_DTL_TYP ,	[A_CODE]=@A_CODE ,
			   [W_CODE]=@W_CODE ,	[A_DESC]=@A_DESC ,	[BRN_NO]=@BRN_NO ,[BILL_HUNG]=@BILL_HUNG ,	[SOURC_BILL_NO]=@SOURC_BILL_NO ,
			   [SOURC_BILL_TYP]=@SOURC_BILL_TYP ,	[BILL_CASH]=@BILL_CASH ,	[CASH_NO]=@CASH_NO ,	[BILL_BANK]=@BILL_BANK ,	[BANK_NO]=@BANK_NO ,
			   [BILL_DR_ACCOUNT]=@BILL_DR_ACCOUNT ,	[BILL_RT_AMT]=@BILL_RT_AMT ,	[PRNT_NO]=@PRNT_NO ,	[OLD_DOC_SER]=@OLD_DOC_SER ,	
			   [CERTIFIED]=@CERTIFIED ,	[CERTIFIED_U_ID]=@CERTIFIED_U_ID ,	[CERTIFIED_DATE]=@CERTIFIED_DATE ,[CERTIFIED_NOTES]=@CERTIFIED_NOTES ,	
			   [CERTIFIED_USED]=@CERTIFIED_USED,[UP_U_ID]=@UP_U_ID ,	[UP_DATE]=@UP_DATE,[UP_TRMNL_NM]=@UP_TRMNL_NM,[AR_TYPE]=@AR_TYPE
			   WHERE BILL_GUID=@BILL_GUID
                
				
               END
			 IF(@Mode='ItemUpdate')

               BEGIN  
			 
			   DECLARE @Item_BILL_SER_U numeric(27, 0)  = NULL
			   DECLARE @Item_BILL_DOC_TYPE_U int        = NULL
			   DECLARE @Item_BILL_NO_U numeric(27, 0)   = NULL
			   DECLARE @Item_P_QTY_U float              = NULL
			   DECLARE @Item_VAT_AMT_U float            = NULL
			   DECLARE @Item_AP_TYPE_U int              = NULL
			   DECLARE @Item_RCRD_NO_U int              = NULL
			   DECLARE @Chk_CASH_NO_U  int              = NULL
			   ------From Master--------
			   SELECT @Item_BILL_SER_U=BILL_SER from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_DOC_TYPE_U=BILL_DOC_TYPE from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_BILL_NO_U=BILL_NO from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			   SELECT @Item_AP_TYPE_U=AR_TYPE from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
			 
			   ------------------------
			   SELECT @Item_RCRD_NO_U=MAX(RCRD_NO) from  NS_AR_BILL_SHOW_PRICE_DLT where BILL_GUID=@Item_BILL_GUID
			    IF(@Item_RCRD_NO_U IS NULL)
				BEGIN
				SET @Item_RCRD_NO_U= 0
				END
				ELSE
				BEGIN
				SET @Item_RCRD_NO_U= @Item_RCRD_NO_U+1
				END
			   --------------------------
			    IF(@Item_I_QTY IS NOT NULL AND @Item_I_QTY > 0)
				BEGIN
				SET @Item_P_QTY_U= @Item_I_QTY * @Item_P_SIZE
				END
				IF(@Item_VAT_PER IS NOT NULL AND @Item_VAT_PER > 0)
				BEGIN
				SET @Item_VAT_AMT_U= (@Item_I_PRICE - @Item_DIS_AMT) * (@Item_VAT_PER/100)
				END
				IF EXISTS(SELECT 1  FROM NS_AR_BILL_SHOW_PRICE_DLT WHERE BILL_DTL_GUID=@Item_BILL_DTL_GUID AND BILL_GUID=@Item_BILL_GUID)
				BEGIN
				UPDATE NS_AR_BILL_SHOW_PRICE_DLT SET [BILL_SER]=@Item_BILL_SER_U,[BILL_DOC_TYPE]=@Item_BILL_DOC_TYPE_U ,[BILL_NO]=@Item_BILL_NO_U,
				[I_CODE]=@Item_I_CODE ,	[I_QTY]=@Item_I_QTY ,	[ITM_UNT]=@Item_ITM_UNT ,	[P_SIZE]=@Item_P_SIZE ,	[P_QTY] =@Item_P_QTY_U,
				[I_PRICE]=@Item_I_PRICE ,	[STK_COST]=@Item_STK_COST ,	[W_CODE]=@Item_W_CODE ,	[EXPIRE_DATE]=GETDATE() ,	[VAT_PER]=@Item_VAT_PER ,	
				[VAT_AMT]=@Item_VAT_AMT_U ,[DIS_PER]=@Item_DIS_PER ,	[DIS_AMT]=@Item_DIS_AMT ,[FREE_QTY] =@Item_FREE_QTY,	[BARCODE]=@Item_BARCODE ,[AR_TYPE]=@Item_AR_TYPE,
				Field1=@Item_Field1,Field2=@Item_Field2,Field3=@Item_Field3,Field4=@Item_Field4,Field5=@Item_Field5,Field6=@Item_Field6,
				Field7=@Item_Field7,Field8=@Item_Field8,Field9=@Item_Field9,Field10=@Item_Field10
				WHERE BILL_DTL_GUID=@Item_BILL_DTL_GUID AND BILL_GUID=@Item_BILL_GUID
				END
				ELSE
				BEGIN
				  SELECT @Item_BILL_SER_U=BILL_SER from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@Item_BILL_GUID
				  DECLARE @BILL_SHOW_PRICE_DLT_GUID_UP uniqueidentifier 
                      SET @BILL_SHOW_PRICE_DLT_GUID_UP = NEWID()
                         INSERT INTO NS_AR_BILL_SHOW_PRICE_DLT
                       (
						[BILL_SER],	[BILL_DOC_TYPE] ,	[BILL_NO] ,	[I_CODE] ,	[I_QTY] ,	[ITM_UNT] ,	[P_SIZE] ,	[P_QTY] ,
						[I_PRICE] ,	[STK_COST] ,	[W_CODE] ,	[EXPIRE_DATE] ,	[VAT_PER] ,	[VAT_AMT] ,	[RCRD_NO] ,
						[DIS_PER] ,	[DIS_AMT] ,	[FREE_QTY] ,	[BARCODE] ,	[AR_TYPE] ,	[BILL_DTL_GUID] ,	[BILL_GUID],Field1,Field2,
						Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10
                       )
                       VALUES
                       (
                        @Item_BILL_SER_U ,	@Item_BILL_DOC_TYPE_U ,	@Item_BILL_NO_U ,	@Item_I_CODE ,	@Item_I_QTY ,	@Item_ITM_UNT ,	@Item_P_SIZE ,	@Item_P_QTY_U ,
						@Item_I_PRICE ,	@Item_STK_COST ,	@Item_W_CODE ,	GETDATE(),	@Item_VAT_PER ,	@Item_VAT_AMT_U ,	@Item_RCRD_NO_U ,	
						@Item_DIS_PER ,	@Item_DIS_AMT ,	@Item_FREE_QTY ,	@Item_BARCODE ,	@Item_AR_TYPE  ,	@BILL_SHOW_PRICE_DLT_GUID_UP ,@Item_BILL_GUID,
						@Item_Field1,@Item_Field2,@Item_Field3,@Item_Field4,@Item_Field5,@Item_Field6,@Item_Field7,@Item_Field8,@Item_Field9,@Item_Field10
                       )
               END
					 -----------------------------Update Amount--------------------------------
					    DECLARE @VAT_AMT_UP decimal(18,2)                 = 0
						DECLARE @BILL_AMT_UP decimal(18,2)                 = 0
						DECLARE @DISC_AMT_UP float                = 0
						DECLARE @DISC_AMT_DTL_UP float            = 0
						DECLARE @ITEM_ROW_AMT_UP float            = 0
						DECLARE @CAL_I_QTY_UP float               = 0
						DECLARE @CAL_I_PRICE_UP float             = 0
						DECLARE @CAL_DISC_AMT_UP float            = 0
						DECLARE @CalDisMst_UP float               = 0
						DECLARE @CalDisDtl_UP float               = 0
			
						DECLARE @cnt_UP INT = 1;
					
						WHILE @cnt_UP <= (SELECT COUNT (BILL_SER) FROM NS_AR_BILL_SHOW_PRICE_DLT where BILL_GUID=@Item_BILL_GUID)
						BEGIN
							
						    WITH Quotation AS ( select * from(select ROW_NUMBER() OVER(order by BILL_SER asc) as  Rownumber,I_QTY,I_PRICE,DIS_AMT FROM NS_AR_BILL_SHOW_PRICE_DLT 
                            where BILL_GUID=@Item_BILL_GUID)z)select @CAL_I_QTY_UP=I_QTY,@CAL_I_PRICE_UP=I_PRICE,@CAL_DISC_AMT_UP=DIS_AMT from Quotation 
							where Rownumber=@cnt_UP
	
							SET @BILL_AMT_UP = @BILL_AMT_UP+(@CAL_I_QTY_UP * @CAL_I_PRICE_UP) 
							SET @DISC_AMT_DTL_UP= @DISC_AMT_DTL_UP +(@CAL_I_QTY_UP * @CAL_DISC_AMT_UP) 
						
							SELECT @CalDisMst_UP=DISC_AMT_MST,@CalDisDtl_UP=DISC_AMT_DTL
							FROM  NS_AR_BILL_SHOW_PRICE_MST 
							where BILL_GUID=@Item_BILL_GUID
							IF(@CalDisMst_UP is null)
							Begin
							set @CalDisMst_UP=0
							End
							IF(@CalDisDtl_UP is null)
							Begin
							set @CalDisDtl_UP=0
							End

							SET @DISC_AMT_UP=@DISC_AMT_UP + (@CalDisMst_UP+@CalDisDtl_UP)
							print @DISC_AMT_DTL_UP

							SET @VAT_AMT_UP =((@BILL_AMT_UP - @DISC_AMT_DTL_UP) * 0.15)

							
							UPDATE NS_AR_BILL_SHOW_PRICE_MST set BILL_AMT = @BILL_AMT_UP,DISC_AMT_DTL=@DISC_AMT_DTL_UP,DISC_AMT=@DISC_AMT_DTL_UP ,VAT_AMT=@VAT_AMT_UP

							WHERE  BILL_GUID=@Item_BILL_GUID;

							SET @cnt_UP = @cnt_UP + 1;
							print @VAT_AMT_UP
						END;

					 -----------------------------------------------------------------------------
	
               END
          IF(@Mode='EditQuo')

               BEGIN  
			
                 select * from NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@BILL_GUID

                 select * from  NS_AR_BILL_SHOW_PRICE_DLT where BILL_GUID=@BILL_GUID   
				
               END
          IF(@Mode='DeleteQuo')
               BEGIN  
			 
			     Delete from  NS_AR_BILL_SHOW_PRICE_DLT where BILL_GUID=@BILL_GUID 
                 Delete from NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@BILL_GUID
				 select 0
               END
		 IF(@Mode='ItemDetail')

               BEGIN  
			  
                 select * from  NS_AR_BILL_SHOW_PRICE_MST where BILL_GUID=@BILL_GUID   
				
               END
END
GO
