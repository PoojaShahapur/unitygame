local UIFormID = 
{
    
}

GlobalNS['UIFOrmID'] = UIFormID;

GlobalNS['FormUIDBase'] = 0;

local addFormId = function(curId)
    GlobalNS.UIFormID[curId] = GlobalNS['FormUIDBase'];
    GlobalNS['FormUIDBase'] = GlobalNS['FormUIDBase'] + 1;
end

addFormId('eUIFormTest');

addFormId('eUICount');