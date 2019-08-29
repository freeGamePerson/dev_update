﻿/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System;
using XLua;
using System.Collections.Generic;



public class LuaCallCs : MonoBehaviour
{
	LuaEnv luaenv = null;
	string script = @"
        function demo()
            --new C#对象
            local newGameObj = CS.UnityEngine.GameObject()
            local newGameObj2 = CS.UnityEngine.GameObject('helloworld')
            print(newGameObj, newGameObj2)
        
            --访问静态属性，方法
            local GameObject = CS.UnityEngine.GameObject
            print('UnityEngine.Time.deltaTime:', CS.UnityEngine.Time.deltaTime) --读静态属性
            CS.UnityEngine.Time.timeScale = 0.5 --写静态属性
            print('helloworld', GameObject.Find('helloworld')) --静态方法调用

            --访问成员属性，方法
            local DerivedClass = CS.Tutorial.DerivedClass
            local testobj = DerivedClass()
            testobj.DMF = 1024--设置成员属性
            print(testobj.DMF)--读取成员属性
            testobj:DMFunc()--成员方法

            --基类属性，方法
            print(DerivedClass.BSF)--读基类静态属性
            DerivedClass.BSF = 2048--写基类静态属性
            DerivedClass.BSFunc();--基类静态方法
            print(testobj.BMF)--读基类成员属性
            testobj.BMF = 4096--写基类成员属性
            testobj:BMFunc()--基类方法调用

            --复杂方法调用
            local ret, p2, p3, csfunc = testobj:ComplexFunc({x=3, y = 'john'}, 100, function()
               print('i am lua callback')
            end)
            print('ComplexFunc ret:', ret, p2, p3, csfunc)
            csfunc()

            --重载方法调用
            testobj:TestFunc(100)
            testobj:TestFunc('hello')

            --操作符
            local testobj2 = DerivedClass()
            testobj2.DMF = 2048
            print('(testobj + testobj2).DMF = ', (testobj + testobj2).DMF)

            --默认值
            testobj:DefaultValueFunc(1)
            testobj:DefaultValueFunc(3, 'hello', 'john')

            --可变参数
            testobj:VariableParamsFunc(5, 'hello', 'john')

            --Extension methods
            print(testobj:GetSomeData()) 
            print(testobj:GetSomeBaseData()) --访问基类的Extension methods
            testobj:GenericMethodOfString()  --通过Extension methods实现访问泛化方法

            --枚举类型
            local e = testobj:EnumTestFunc(CS.Tutorial.TestEnum.E1)
            print(e, e == CS.Tutorial.TestEnum.E2)
            print(CS.Tutorial.TestEnum.__CastFrom(1), CS.Tutorial.TestEnum.__CastFrom('E1'))
            print(CS.Tutorial.DerivedClass.TestEnumInner.E3)
            assert(CS.Tutorial.BaseClass.TestEnumInner == nil)

            --Delegate
            testobj.TestDelegate('hello') --直接调用
            local function lua_delegate(str)
                print('TestDelegate in lua:', str)
            end
            testobj.TestDelegate = lua_delegate + testobj.TestDelegate --combine，这里演示的是C#delegate作为右值，左值也支持
            testobj.TestDelegate('hello')
            testobj.TestDelegate = testobj.TestDelegate - lua_delegate --remove
            testobj.TestDelegate('hello')

            --事件
            local function lua_event_callback1() print('lua_event_callback1') end
            local function lua_event_callback2() print('lua_event_callback2') end
            testobj:TestEvent('+', lua_event_callback1)
            testobj:CallEvent()
            testobj:TestEvent('+', lua_event_callback2)
            testobj:CallEvent()
            testobj:TestEvent('-', lua_event_callback1)
            testobj:CallEvent()
            testobj:TestEvent('-', lua_event_callback2)

            --64位支持
            local l = testobj:TestLong(11)
            print(type(l), l, l + 100, 10000 + l)

            --typeof
            newGameObj:AddComponent(typeof(CS.UnityEngine.ParticleSystem))

            --cast
            local calc = testobj:GetCalc()
            print('assess instance of InnerCalc via reflection', calc:add(1, 2))
            assert(calc.id == 100)
            cast(calc, typeof(CS.Tutorial.ICalc))
            print('cast to interface ICalc', calc:add(1, 2))
            assert(calc.id == nil)
       end

       demo()

       --协程下使用
       local co = coroutine.create(function()
           print('------------------------------------------------------')
           demo()
       end)
       assert(coroutine.resume(co))
    ";

	// Use this for initialization
	void Start()
	{
		luaenv = new LuaEnv();
		luaenv.DoString(script);
	}

	// Update is called once per frame
	void Update()
	{
		if (luaenv != null)
		{
			luaenv.Tick();
		}
	}

	void OnDestroy()
	{
		luaenv.Dispose();
	}
}
