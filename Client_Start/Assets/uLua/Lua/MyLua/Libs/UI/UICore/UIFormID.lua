local UIFormID = 
{
    
}

GlobalNS['UIFormID'] = UIFormID;

GlobalNS['FormUIDBase'] = 0;

local addFormId = function(curId)
    GlobalNS.UIFormID[curId] = GlobalNS['FormUIDBase'];
    GlobalNS['FormUIDBase'] = GlobalNS['FormUIDBase'] + 1;
end

addFormId('eUITest');

addFormId('eUICount');