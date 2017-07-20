--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "FlyPos";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.pos_3rose_1 = GlobalNS.new(GlobalNS.MList);
    self.pos_3rose_2 = GlobalNS.new(GlobalNS.MList);
    self.pos_4rose_1 = GlobalNS.new(GlobalNS.MList);
    self.pos_4rose_2 = GlobalNS.new(GlobalNS.MList);
    self.pos_arch = GlobalNS.new(GlobalNS.MList);

    self.radius = 150;
end

function M:dtor()
	self.pos_3rose_1:Clear();
    self.pos_3rose_2:Clear();
    self.pos_4rose_1:Clear();
    self.pos_4rose_2:Clear();
    self.pos_arch:Clear();
end

function M:init()
    self:calc3rosedosventor();
    self:calc3rosedosventor_2();
    self:calc4rosedosventor();
    self:calc4rosedosventor_2();
    self:ArchimedeanSpiral();
end

--三叶玫瑰线，可以用来做移动动画（x，y）
function M:calc3rosedosventor()
    local r1 = 0;
    local x1 = 0;
    local y1 = 0;
    local radian = 0;
    for t = 0, math.pi + 0.01, 0.05 do
        radian = 3 * t; --转向弧度
        r1 = self.radius * math.sin(radian);  --半径
        x1 = r1 * math.cos(t);
        y1 = r1 * math.sin(t);
        
        self.pos_3rose_1:add(Vector2.New(x1, y1));
    end
end

--[[
对任意点(x,y)，绕一个坐标点(rx0,ry0)逆时针旋转a角度后的新的坐标设为(x0, y0)，有公式：
x0= (x - rx0)*cos(a) - (y - ry0)*sin(a) + rx0 ;
y0= (x - rx0)*sin(a) + (y - ry0)*cos(a) + ry0 ;
]]--
function M:calc3rosedosventor_2()
    --把pos_3rose_1中的所有点绕原点(0，0)逆时针旋转60度
    local count = self.pos_3rose_1:Count() - 1;
    local delta = 1 / 3;
    for t=0, count do
        local pos = self.pos_3rose_1:get(t);
        local x = pos.x * math.cos(delta * math.pi) - pos.y * math.sin(delta * math.pi);
        local y = pos.x * math.sin(delta * math.pi) + pos.y * math.cos(delta * math.pi);

        self.pos_3rose_2:add(Vector2.New(x, y));
    end
end

--四叶玫瑰线
function M:calc4rosedosventor()
    local r1 = 0;
    local x1 = 0;
    local y1 = 0;
    local radian = 0;
    for t = 0, 2 * math.pi + 0.02, 0.1 do
        radian = 2 * t; --转向弧度
        r1 = self.radius * math.sin(radian);  --半径
        x1 = r1 * math.cos(t);
        y1 = r1 * math.sin(t);
        
        self.pos_4rose_1:add(Vector2.New(x1, y1));
    end
end

function M:calc4rosedosventor_2()
    --把pos_4rose_1中的所有点绕原点(0，0)逆时针旋转45度
    local count = self.pos_4rose_1:Count() - 1;
    for t=0, count do
        local pos = self.pos_4rose_1:get(t);
        local x = pos.x * math.cos(0.25 * math.pi) - pos.y * math.sin(0.25 * math.pi);
        local y = pos.x * math.sin(0.25 * math.pi) + pos.y * math.cos(0.25 * math.pi);

        self.pos_4rose_2:add(Vector2.New(x, y));
    end
end

--阿基米德螺线
function M:ArchimedeanSpiral()
    local r1 = 0;
    local x1 = 0;
    local y1 = 0;
    for t = 0, 2 * math.pi, 0.1 do
        r1 = 40 * t;  --半径
        x1 = r1 * math.cos(2 * t);
        y1 = r1 * math.sin(2 * t);
        
        --绕原点(0，0)逆时针旋转90度
        local x = x1 * math.cos(0.5 * math.pi) - y1 * math.sin(0.5 * math.pi);
        local y = x1 * math.sin(0.5 * math.pi) + y1 * math.cos(0.5 * math.pi);

        self.pos_arch:add(Vector2.New(x, y));
    end
end

return M;

--endregion
