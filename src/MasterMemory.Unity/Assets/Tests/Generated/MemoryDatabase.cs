﻿using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using MasterMemory.Tests.Tables;

namespace MasterMemory.Tests
{
   public sealed class MemoryDatabase : MemoryDatabaseBase
   {
        public SampleTable SampleTable { get; private set; }
        public SkillMasterTable SkillMasterTable { get; private set; }
        public UserLevelTable UserLevelTable { get; private set; }

        public MemoryDatabase(
            SampleTable SampleTable,
            SkillMasterTable SkillMasterTable,
            UserLevelTable UserLevelTable
        )
        {
            this.SampleTable = SampleTable;
            this.SkillMasterTable = SkillMasterTable;
            this.UserLevelTable = UserLevelTable;
        }

        public MemoryDatabase(byte[] databaseBinary, bool internString = true, IFormatterResolver formatterResolver = null)
            : base(databaseBinary, internString, formatterResolver)
        {
        }

        protected override void Init(Dictionary<string, (int offset, int count)> header, int headerOffset, byte[] databaseBinary, IFormatterResolver resolver)
        {
            this.SampleTable = ExtractTableData<Sample, SampleTable>(header, headerOffset, databaseBinary, resolver, xs => new SampleTable(xs));
            this.SkillMasterTable = ExtractTableData<SkillMaster, SkillMasterTable>(header, headerOffset, databaseBinary, resolver, xs => new SkillMasterTable(xs));
            this.UserLevelTable = ExtractTableData<UserLevel, UserLevelTable>(header, headerOffset, databaseBinary, resolver, xs => new UserLevelTable(xs));
        }

        public ImmutableBuilder ToImmutableBuilder()
        {
            return new ImmutableBuilder(this);
        }

        public DatabaseBuilder ToDatabaseBuilder()
        {
            var builder = new DatabaseBuilder();
            builder.Append(this.SampleTable.GetRawDataUnsafe());
            builder.Append(this.SkillMasterTable.GetRawDataUnsafe());
            builder.Append(this.UserLevelTable.GetRawDataUnsafe());
            return builder;
        }
    }
}