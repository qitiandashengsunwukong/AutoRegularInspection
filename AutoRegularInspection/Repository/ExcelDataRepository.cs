﻿using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Repository
{
    public class ExcelDataRepository:IDataRepository
    {
        /// <summary>
        /// 读取病害数据
        /// </summary>
        /// <param name="workSheetName">工作簿名称</param>
        /// <returns></returns>
        public List<DamageSummary> ReadDamageData(BridgePart bridgePart)
        {
            string strFilePath = App.DamageSummaryFileName;
            var workSheetName = Services.EnumHelper.GetEnumDesc(bridgePart).ToString();
            var lst = new List<DamageSummary>();

            if (!File.Exists(strFilePath))
            {
                return lst;
            }

            try
            {

                FileInfo file = new FileInfo(strFilePath);

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[workSheetName];
                    int rowCount = 2;// worksheet.Dimension.Rows;   //worksheet.Dimension.Rows指的是所有列中最大行
                    //首行：表头不导入
                    bool rowCur = true;    //行游标指示器
                                           //rowCur=false表示到达行尾
                                           //计算行数
                    while (rowCur)
                    {
                        try
                        {
                            //跳过表头
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[rowCount + 1, 1].Value.ToString()))
                            {
                                rowCur = false;
                            }
                        }
                        catch (Exception)   //读取异常则终止
                        {
                            rowCur = false;
                        }

                        if (rowCur)
                        {
                            rowCount++;
                        }
                    }

                    //bool validationResult = false;
                    int row = 2;    //excel中行指针
                    //行号不为空，则继续添加
                    //while (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Value.ToString()))
                    for (row = 2; row <= rowCount; row++)
                    {
                        //
                        //1、处理excel数据导入;
                        //2、验证"视图模型";
                        //3、验证业务模型;
                        try
                        {
                            lst.Add(new DamageSummary
                            {
                                No = row - 1,
                                Position = worksheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                                Component = worksheet.Cells[row, 3].Value?.ToString() ?? string.Empty,
                                Damage = worksheet.Cells[row, 4].Value?.ToString() ?? string.Empty,
                                DamageDescription = worksheet.Cells[row, 5].Value?.ToString() ?? string.Empty,
                                DamageDescriptionInPicture = worksheet.Cells[row, 6].Value?.ToString() ?? string.Empty,
                                PictureNo = worksheet.Cells[row, 7].Value?.ToString() ?? string.Empty,
                            });
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                }
                //显示导入结果
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
