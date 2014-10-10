/*
 * jQuery JavaScript Library v1.4.2
 * http://jquery.com/
 *
 * Copyright 2010, John Resig
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * Includes Sizzle.js
 * http://sizzlejs.com/
 * Copyright 2010, The Dojo Foundation
 * Released under the MIT, BSD, and GPL Licenses.
 *
 * Date: Sat Feb 13 22:33:48 2010 -0500
 */
(function (aM, C) {
		var a = function (aY, aZ) {
			return new a.fn.init(aY, aZ);
		},
		n = aM.jQuery,
		R = aM.$,
		ab = aM.document,
		X,
		P = /^[^<]*(<[\w\W]+>)[^>]*$|^#([\w-]+)$/,
		aW = /^.[^:#\[\.,]*$/,
		ax = /\S/,
		M = /^(\s|\u00A0)+|(\s|\u00A0)+$/g,
		e = /^<(\w+)\s*\/?>(?:<\/\1>)?$/,
		b = navigator.userAgent,
		u,
		K = false,
		ad = [],
		aG,
		at = Object.prototype.toString,
		ap = Object.prototype.hasOwnProperty,
		g = Array.prototype.push,
		F = Array.prototype.slice,
		s = Array.prototype.indexOf;
		a.fn = a.prototype = {
			init : function (aY, a1) {
				var a0,
				a2,
				aZ,
				a3;
				if (!aY) {
					return this;
				}
				if (aY.nodeType) {
					this.context = this[0] = aY;
					this.length = 1;
					return this;
				}
				if (aY === "body" && !a1) {
					this.context = ab;
					this[0] = ab.body;
					this.selector = "body";
					this.length = 1;
					return this;
				}
				if (typeof aY === "string") {
					a0 = P.exec(aY);
					if (a0 && (a0[1] || !a1)) {
						if (a0[1]) {
							a3 = (a1 ? a1.ownerDocument || a1 : ab);
							aZ = e.exec(aY);
							if (aZ) {
								if (a.isPlainObject(a1)) {
									aY = [ab.createElement(aZ[1])];
									a.fn.attr.call(aY, a1, true);
								} else {
									aY = [a3.createElement(aZ[1])];
								}
							} else {
								aZ = J([a0[1]], [a3]);
								aY = (aZ.cacheable ? aZ.fragment.cloneNode(true) : aZ.fragment).childNodes;
							}
							return a.merge(this, aY);
						} else {
							a2 = ab.getElementById(a0[2]);
							if (a2) {
								if (a2.id !== a0[2]) {
									return X.find(aY);
								}
								this.length = 1;
								this[0] = a2;
							}
							this.context = ab;
							this.selector = aY;
							return this;
						}
					} else {
						if (!a1 && /^\w+$/.test(aY)) {
							this.selector = aY;
							this.context = ab;
							aY = ab.getElementsByTagName(aY);
							return a.merge(this, aY);
						} else {
							if (!a1 || a1.jquery) {
								return(a1 || X).find(aY);
							} else {
								return a(a1).find(aY);
							}
						}
					}
				} else {
					if (a.isFunction(aY)) {
						return X.ready(aY);
					}
				}
				if (aY.selector !== C) {
					this.selector = aY.selector;
					this.context = aY.context;
				}
				return a.makeArray(aY, this);
			},
			selector : "",
			jquery : "1.4.2",
			length : 0,
			size : function () {
				return this.length;
			},
			toArray : function () {
				return F.call(this, 0);
			},
			get : function (aY) {
				return aY == null ? this.toArray() : (aY < 0 ? this.slice(aY)[0] : this[aY]);
			},
			pushStack : function (aZ, a1, aY) {
				var a0 = a();
				if (a.isArray(aZ)) {
					g.apply(a0, aZ);
				} else {
					a.merge(a0, aZ);
				}
				a0.prevObject = this;
				a0.context = this.context;
				if (a1 === "find") {
					a0.selector = this.selector + (this.selector ? " " : "") + aY;
				} else {
					if (a1) {
						a0.selector = this.selector + "." + a1 + "(" + aY + ")";
					}
				}
				return a0;
			},
			each : function (aZ, aY) {
				return a.each(this, aZ, aY);
			},
			ready : function (aY) {
				a.bindReady();
				if (a.isReady) {
					aY.call(ab, a);
				} else {
					if (ad) {
						ad.push(aY);
					}
				}
				return this;
			},
			eq : function (aY) {
				return aY === -1 ? this.slice(aY) : this.slice(aY, +aY + 1);
			},
			first : function () {
				return this.eq(0);
			},
			last : function () {
				return this.eq(-1);
			},
			slice : function () {
				return this.pushStack(F.apply(this, arguments), "slice", F.call(arguments).join(","));
			},
			map : function (aY) {
				return this.pushStack(a.map(this, function (a0, aZ) {
							return aY.call(a0, aZ, a0);
						}));
			},
			end : function () {
				return this.prevObject || a(null);
			},
			push : g,
			sort : [].sort,
			splice : [].splice
		};
		a.fn.init.prototype = a.fn;
		a.extend = a.fn.extend = function () {
			var a3 = arguments[0] || {
			},
			a2 = 1,
			a1 = arguments.length,
			a5 = false,
			a6,
			a0,
			aY,
			aZ;
			if (typeof a3 === "boolean") {
				a5 = a3;
				a3 = arguments[1] || {
				};
				a2 = 2;
			}
			if (typeof a3 !== "object" && !a.isFunction(a3)) {
				a3 = {
				};
			}
			if (a1 === a2) {
				a3 = this;
				--a2;
			}
			for (; a2 < a1; a2++) {
				if ((a6 = arguments[a2]) != null) {
					for (a0 in a6) {
						aY = a3[a0];
						aZ = a6[a0];
						if (a3 === aZ) {
							continue;
						}
						if (a5 && aZ && (a.isPlainObject(aZ) || a.isArray(aZ))) {
							var a4 = aY && (a.isPlainObject(aY) || a.isArray(aY)) ? aY : a.isArray(aZ) ? [] : {
							};
							a3[a0] = a.extend(a5, a4, aZ);
						} else {
							if (aZ !== C) {
								a3[a0] = aZ;
							}
						}
					}
				}
			}
			return a3;
		};
		a.extend({
				noConflict : function (aY) {
					aM.$ = R;
					if (aY) {
						aM.jQuery = n;
					}
					return a;
				},
				isReady : false,
				ready : function () {
					if (!a.isReady) {
						if (!ab.body) {
							return setTimeout(a.ready, 13);
						}
						a.isReady = true;
						if (ad) {
							var aZ,
							aY = 0;
							while ((aZ = ad[aY++])) {
								aZ.call(ab, a);
							}
							ad = null;
						}
						if (a.fn.triggerHandler) {
							a(ab).triggerHandler("ready");
						}
					}
				},
				bindReady : function () {
					if (K) {
						return;
					}
					K = true;
					if (ab.readyState === "complete") {
						return a.ready();
					}
					if (ab.addEventListener) {
						ab.addEventListener("DOMContentLoaded", aG, false);
						aM.addEventListener("load", a.ready, false);
					} else {
						if (ab.attachEvent) {
							ab.attachEvent("onreadystatechange", aG);
							aM.attachEvent("onload", a.ready);
							var aY = false;
							try {
								aY = aM.frameElement == null;
							} catch (aZ) {
							}
							if (ab.documentElement.doScroll && aY) {
								x();
							}
						}
					}
				},
				isFunction : function (aY) {
					return at.call(aY) === "[object Function]";
				},
				isArray : function (aY) {
					return at.call(aY) === "[object Array]";
				},
				isPlainObject : function (aZ) {
					if (!aZ || at.call(aZ) !== "[object Object]" || aZ.nodeType || aZ.setInterval) {
						return false;
					}
					if (aZ.constructor && !ap.call(aZ, "constructor") && !ap.call(aZ.constructor.prototype, "isPrototypeOf")) {
						return false;
					}
					var aY;
					for (aY in aZ) {
					}
					return aY === C || ap.call(aZ, aY);
				},
				isEmptyObject : function (aZ) {
					for (var aY in aZ) {
						return false;
					}
					return true;
				},
				error : function (aY) {
					throw aY;
				},
				parseJSON : function (aY) {
					if (typeof aY !== "string" || !aY) {
						return null;
					}
					aY = a.trim(aY);
					if (/^[\],:{}\s]*$/.test(aY.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) {
						return aM.JSON && aM.JSON.parse ? aM.JSON.parse(aY) : (new Function("return " + aY))();
					} else {
						a.error("Invalid JSON: " + aY);
					}
				},
				noop : function () {
				},
				globalEval : function (a0) {
					if (a0 && ax.test(a0)) {
						var aZ = ab.getElementsByTagName("head")[0] || ab.documentElement,
						aY = ab.createElement("script");
						aY.type = "text/javascript";
						if (a.support.scriptEval) {
							aY.appendChild(ab.createTextNode(a0));
						} else {
							aY.text = a0;
						}
						aZ.insertBefore(aY, aZ.firstChild);
						aZ.removeChild(aY);
					}
				},
				nodeName : function (aZ, aY) {
					return aZ.nodeName && aZ.nodeName.toUpperCase() === aY.toUpperCase();
				},
				each : function (a1, a5, a0) {
					var aZ,
					a2 = 0,
					a3 = a1.length,
					aY = a3 === C || a.isFunction(a1);
					if (a0) {
						if (aY) {
							for (aZ in a1) {
								if (a5.apply(a1[aZ], a0) === false) {
									break;
								}
							}
						} else {
							for (; a2 < a3; ) {
								if (a5.apply(a1[a2++], a0) === false) {
									break;
								}
							}
						}
					} else {
						if (aY) {
							for (aZ in a1) {
								if (a5.call(a1[aZ], aZ, a1[aZ]) === false) {
									break;
								}
							}
						} else {
							for (var a4 = a1[0]; a2 < a3 && a5.call(a4, a2, a4) !== false; a4 = a1[++a2]) {
							}
						}
					}
					return a1;
				},
				trim : function (aY) {
					return(aY || "").replace(M, "");
				},
				makeArray : function (a0, aZ) {
					var aY = aZ || [];
					if (a0 != null) {
						if (a0.length == null || typeof a0 === "string" || a.isFunction(a0) || (typeof a0 !== "function" && a0.setInterval)) {
							g.call(aY, a0);
						} else {
							a.merge(aY, a0);
						}
					}
					return aY;
				},
				inArray : function (a0, a1) {
					if (a1.indexOf) {
						return a1.indexOf(a0);
					}
					for (var aY = 0, aZ = a1.length; aY < aZ; aY++) {
						if (a1[aY] === a0) {
							return aY;
						}
					}
					return - 1;
				},
				merge : function (a2, a0) {
					var a1 = a2.length,
					aZ = 0;
					if (typeof a0.length === "number") {
						for (var aY = a0.length; aZ < aY; aZ++) {
							a2[a1++] = a0[aZ];
						}
					} else {
						while (a0[aZ] !== C) {
							a2[a1++] = a0[aZ++];
						}
					}
					a2.length = a1;
					return a2;
				},
				grep : function (aZ, a3, aY) {
					var a0 = [];
					for (var a1 = 0, a2 = aZ.length; a1 < a2; a1++) {
						if (!aY !== !a3(aZ[a1], a1)) {
							a0.push(aZ[a1]);
						}
					}
					return a0;
				},
				map : function (aZ, a4, aY) {
					var a0 = [],
					a3;
					for (var a1 = 0, a2 = aZ.length; a1 < a2; a1++) {
						a3 = a4(aZ[a1], a1, aY);
						if (a3 != null) {
							a0[a0.length] = a3;
						}
					}
					return a0.concat.apply([], a0);
				},
				guid : 1,
				proxy : function (a0, aZ, aY) {
					if (arguments.length === 2) {
						if (typeof aZ === "string") {
							aY = a0;
							a0 = aY[aZ];
							aZ = C;
						} else {
							if (aZ && !a.isFunction(aZ)) {
								aY = aZ;
								aZ = C;
							}
						}
					}
					if (!aZ && a0) {
						aZ = function () {
							return a0.apply(aY || this, arguments);
						};
					}
					if (a0) {
						aZ.guid = a0.guid = a0.guid || aZ.guid || a.guid++;
					}
					return aZ;
				},
				uaMatch : function (aZ) {
					aZ = aZ.toLowerCase();
					var aY = /(webkit)[ \/]([\w.]+)/.exec(aZ) || /(opera)(?:.*version)?[ \/]([\w.]+)/.exec(aZ) || /(msie) ([\w.]+)/.exec(aZ) || !/compatible/.test(aZ) && /(mozilla)(?:.*? rv:([\w.]+))?/.exec(aZ) || [];
					return{
						browser : aY[1] || "",
						version : aY[2] || "0"
					};
				},
				browser : {
				}
			});
		u = a.uaMatch(b);
		if (u.browser) {
			a.browser[u.browser] = true;
			a.browser.version = u.version;
		}
		if (a.browser.webkit) {
			a.browser.safari = true;
		}
		if (s) {
			a.inArray = function (aY, aZ) {
				return s.call(aZ, aY);
			};
		}
		X = a(ab);
		if (ab.addEventListener) {
			aG = function () {
				ab.removeEventListener("DOMContentLoaded", aG, false);
				a.ready();
			};
		} else {
			if (ab.attachEvent) {
				aG = function () {
					if (ab.readyState === "complete") {
						ab.detachEvent("onreadystatechange", aG);
						a.ready();
					}
				};
			}
		}
		function x() {
			if (a.isReady) {
				return;
			}
			try {
				ab.documentElement.doScroll("left");
			} catch (aY) {
				setTimeout(x, 1);
				return;
			}
			a.ready();
		}
		function aV(aY, aZ) {
			if (aZ.src) {
				a.ajax({
						url : aZ.src,
						async : false,
						dataType : "script"
					});
			} else {
				a.globalEval(aZ.text || aZ.textContent || aZ.innerHTML || "");
			}
			if (aZ.parentNode) {
				aZ.parentNode.removeChild(aZ);
			}
		}
		function an(aY, a6, a4, a0, a3, a5) {
			var aZ = aY.length;
			if (typeof a6 === "object") {
				for (var a1 in a6) {
					an(aY, a1, a6[a1], a0, a3, a4);
				}
				return aY;
			}
			if (a4 !== C) {
				a0 = !a5 && a0 && a.isFunction(a4);
				for (var a2 = 0; a2 < aZ; a2++) {
					a3(aY[a2], a6, a0 ? a4.call(aY[a2], a2, a3(aY[a2], a6)) : a4, a5);
				}
				return aY;
			}
			return aZ ? a3(aY[0], a6) : C;
		}
		function aP() {
			return(new Date).getTime();
		}
		(function () {
				a.support = {
				};
				var a4 = ab.documentElement,
				a3 = ab.createElement("script"),
				aY = ab.createElement("div"),
				aZ = "script" + aP();
				aY.style.display = "none";
				aY.innerHTML = "   <link/><table></table><a href='/a' style='color:red;float:left;opacity:.55;'>a</a><input type='checkbox'/>";
				var a6 = aY.getElementsByTagName("*"),
				a5 = aY.getElementsByTagName("a")[0];
				if (!a6 || !a6.length || !a5) {
					return;
				}
				a.support = {
					leadingWhitespace : aY.firstChild.nodeType === 3,
					tbody : !aY.getElementsByTagName("tbody").length,
					htmlSerialize : !!aY.getElementsByTagName("link").length,
					style : /red/.test(a5.getAttribute("style")),
					hrefNormalized : a5.getAttribute("href") === "/a",
					opacity : /^0.55$/.test(a5.style.opacity),
					cssFloat : !!a5.style.cssFloat,
					checkOn : aY.getElementsByTagName("input")[0].value === "on",
					optSelected : ab.createElement("select").appendChild(ab.createElement("option")).selected,
					parentNode : aY.removeChild(aY.appendChild(ab.createElement("div"))).parentNode === null,
					deleteExpando : true,
					checkClone : false,
					scriptEval : false,
					noCloneEvent : true,
					boxModel : null
				};
				a3.type = "text/javascript";
				try {
					a3.appendChild(ab.createTextNode("window." + aZ + "=1;"));
				} catch (a1) {
				}
				a4.insertBefore(a3, a4.firstChild);
				if (aM[aZ]) {
					a.support.scriptEval = true;
					delete aM[aZ];
				}
				try {
					delete a3.test;
				} catch (a1) {
					a.support.deleteExpando = false;
				}
				a4.removeChild(a3);
				if (aY.attachEvent && aY.fireEvent) {
					aY.attachEvent("onclick", function a7() {
							a.support.noCloneEvent = false;
							aY.detachEvent("onclick", a7);
						});
					aY.cloneNode(true).fireEvent("onclick");
				}
				aY = ab.createElement("div");
				aY.innerHTML = "<input type='radio' name='radiotest' checked='checked'/>";
				var a0 = ab.createDocumentFragment();
				a0.appendChild(aY.firstChild);
				a.support.checkClone = a0.cloneNode(true).cloneNode(true).lastChild.checked;
				a(function () {
						var a8 = ab.createElement("div");
						a8.style.width = a8.style.paddingLeft = "1px";
						ab.body.appendChild(a8);
						a.boxModel = a.support.boxModel = a8.offsetWidth === 2;
						ab.body.removeChild(a8).style.display = "none";
						a8 = null;
					});
				var a2 = function (a8) {
					var ba = ab.createElement("div");
					a8 = "on" + a8;
					var a9 = (a8 in ba);
					if (!a9) {
						ba.setAttribute(a8, "return;");
						a9 = typeof ba[a8] === "function";
					}
					ba = null;
					return a9;
				};
				a.support.submitBubbles = a2("submit");
				a.support.changeBubbles = a2("change");
				a4 = a3 = aY = a6 = a5 = null;
			})();
		a.props = {
			"for" : "htmlFor",
			"class" : "className",
			readonly : "readOnly",
			maxlength : "maxLength",
			cellspacing : "cellSpacing",
			rowspan : "rowSpan",
			colspan : "colSpan",
			tabindex : "tabIndex",
			usemap : "useMap",
			frameborder : "frameBorder"
		};
		var aI = "jQuery" + aP(),
		aH = 0,
		aT = {
		};
		a.extend({
				cache : {
				},
				expando : aI,
				noData : {
					embed : true,
					object : true,
					applet : true
				},
				data : function (a0, aZ, a2) {
					if (a0.nodeName && a.noData[a0.nodeName.toLowerCase()]) {
						return;
					}
					a0 = a0 == aM ? aT : a0;
					var a3 = a0[aI],
					aY = a.cache,
					a1;
					if (!a3 && typeof aZ === "string" && a2 === C) {
						return null;
					}
					if (!a3) {
						a3 = ++aH;
					}
					if (typeof aZ === "object") {
						a0[aI] = a3;
						a1 = aY[a3] = a.extend(true, {
							}, aZ);
					} else {
						if (!aY[a3]) {
							a0[aI] = a3;
							aY[a3] = {
							};
						}
					}
					a1 = aY[a3];
					if (a2 !== C) {
						a1[aZ] = a2;
					}
					return typeof aZ === "string" ? a1[aZ] : a1;
				},
				removeData : function (a0, aZ) {
					if (a0.nodeName && a.noData[a0.nodeName.toLowerCase()]) {
						return;
					}
					a0 = a0 == aM ? aT : a0;
					var a2 = a0[aI],
					aY = a.cache,
					a1 = aY[a2];
					if (aZ) {
						if (a1) {
							delete a1[aZ];
							if (a.isEmptyObject(a1)) {
								a.removeData(a0);
							}
						}
					} else {
						if (a.support.deleteExpando) {
							delete a0[a.expando];
						} else {
							if (a0.removeAttribute) {
								a0.removeAttribute(a.expando);
							}
						}
						delete aY[a2];
					}
				}
			});
		a.fn.extend({
				data : function (aY, a0) {
					if (typeof aY === "undefined" && this.length) {
						return a.data(this[0]);
					} else {
						if (typeof aY === "object") {
							return this.each(function () {
									a.data(this, aY);
								});
						}
					}
					var a1 = aY.split(".");
					a1[1] = a1[1] ? "." + a1[1] : "";
					if (a0 === C) {
						var aZ = this.triggerHandler("getData" + a1[1] + "!", [a1[0]]);
						if (aZ === C && this.length) {
							aZ = a.data(this[0], aY);
						}
						return aZ === C && a1[1] ? this.data(a1[0]) : aZ;
					} else {
						return this.trigger("setData" + a1[1] + "!", [a1[0], a0]).each(function () {
								a.data(this, aY, a0);
							});
					}
				},
				removeData : function (aY) {
					return this.each(function () {
							a.removeData(this, aY);
						});
				}
			});
		a.extend({
				queue : function (aZ, aY, a1) {
					if (!aZ) {
						return;
					}
					aY = (aY || "fx") + "queue";
					var a0 = a.data(aZ, aY);
					if (!a1) {
						return a0 || [];
					}
					if (!a0 || a.isArray(a1)) {
						a0 = a.data(aZ, aY, a.makeArray(a1));
					} else {
						a0.push(a1);
					}
					return a0;
				},
				dequeue : function (a1, a0) {
					a0 = a0 || "fx";
					var aY = a.queue(a1, a0),
					aZ = aY.shift();
					if (aZ === "inprogress") {
						aZ = aY.shift();
					}
					if (aZ) {
						if (a0 === "fx") {
							aY.unshift("inprogress");
						}
						aZ.call(a1, function () {
								a.dequeue(a1, a0);
							});
					}
				}
			});
		a.fn.extend({
				queue : function (aY, aZ) {
					if (typeof aY !== "string") {
						aZ = aY;
						aY = "fx";
					}
					if (aZ === C) {
						return a.queue(this[0], aY);
					}
					return this.each(function (a1, a2) {
							var a0 = a.queue(this, aY, aZ);
							if (aY === "fx" && a0[0] !== "inprogress") {
								a.dequeue(this, aY);
							}
						});
				},
				dequeue : function (aY) {
					return this.each(function () {
							a.dequeue(this, aY);
						});
				},
				delay : function (aZ, aY) {
					aZ = a.fx ? a.fx.speeds[aZ] || aZ : aZ;
					aY = aY || "fx";
					return this.queue(aY, function () {
							var a0 = this;
							setTimeout(function () {
									a.dequeue(a0, aY);
								}, aZ);
						});
				},
				clearQueue : function (aY) {
					return this.queue(aY || "fx", []);
				}
			});
		var ao = /[\n\t]/g,
		S = /\s+/,
		av = /\r/g,
		aQ = /href|src|style/,
		d = /(button|input)/i,
		z = /(button|input|object|select|textarea)/i,
		j = /^(a|area)$/i,
		I = /radio|checkbox/;
		a.fn.extend({
				attr : function (aY, aZ) {
					return an(this, aY, aZ, true, a.attr);
				},
				removeAttr : function (aY, aZ) {
					return this.each(function () {
							a.attr(this, aY, "");
							if (this.nodeType === 1) {
								this.removeAttribute(aY);
							}
						});
				},
				addClass : function (a5) {
					if (a.isFunction(a5)) {
						return this.each(function (a8) {
								var a7 = a(this);
								a7.addClass(a5.call(this, a8, a7.attr("class")));
							});
					}
					if (a5 && typeof a5 === "string") {
						var aY = (a5 || "").split(S);
						for (var a1 = 0, a0 = this.length; a1 < a0; a1++) {
							var aZ = this[a1];
							if (aZ.nodeType === 1) {
								if (!aZ.className) {
									aZ.className = a5;
								} else {
									var a2 = " " + aZ.className + " ",
									a4 = aZ.className;
									for (var a3 = 0, a6 = aY.length; a3 < a6; a3++) {
										if (a2.indexOf(" " + aY[a3] + " ") < 0) {
											a4 += " " + aY[a3];
										}
									}
									aZ.className = a.trim(a4);
								}
							}
						}
					}
					return this;
				},
				removeClass : function (a3) {
					if (a.isFunction(a3)) {
						return this.each(function (a7) {
								var a6 = a(this);
								a6.removeClass(a3.call(this, a7, a6.attr("class")));
							});
					}
					if ((a3 && typeof a3 === "string") || a3 === C) {
						var a4 = (a3 || "").split(S);
						for (var a0 = 0, aZ = this.length; a0 < aZ; a0++) {
							var a2 = this[a0];
							if (a2.nodeType === 1 && a2.className) {
								if (a3) {
									var a1 = (" " + a2.className + " ").replace(ao, " ");
									for (var a5 = 0, aY = a4.length; a5 < aY; a5++) {
										a1 = a1.replace(" " + a4[a5] + " ", " ");
									}
									a2.className = a.trim(a1);
								} else {
									a2.className = "";
								}
							}
						}
					}
					return this;
				},
				toggleClass : function (a1, aZ) {
					var a0 = typeof a1,
					aY = typeof aZ === "boolean";
					if (a.isFunction(a1)) {
						return this.each(function (a3) {
								var a2 = a(this);
								a2.toggleClass(a1.call(this, a3, a2.attr("class"), aZ), aZ);
							});
					}
					return this.each(function () {
							if (a0 === "string") {
								var a4,
								a3 = 0,
								a2 = a(this),
								a5 = aZ,
								a6 = a1.split(S);
								while ((a4 = a6[a3++])) {
									a5 = aY ? a5 : !a2.hasClass(a4);
									a2[a5 ? "addClass" : "removeClass"](a4);
								}
							} else {
								if (a0 === "undefined" || a0 === "boolean") {
									if (this.className) {
										a.data(this, "__className__", this.className);
									}
									this.className = this.className || a1 === false ? "" : a.data(this, "__className__") || "";
								}
							}
						});
				},
				hasClass : function (aY) {
					var a1 = " " + aY + " ";
					for (var a0 = 0, aZ = this.length; a0 < aZ; a0++) {
						if ((" " + this[a0].className + " ").replace(ao, " ").indexOf(a1) > -1) {
							return true;
						}
					}
					return false;
				},
				val : function (a5) {
					if (a5 === C) {
						var aZ = this[0];
						if (aZ) {
							if (a.nodeName(aZ, "option")) {
								return(aZ.attributes.value || {
									}).specified ? aZ.value : aZ.text;
							}
							if (a.nodeName(aZ, "select")) {
								var a3 = aZ.selectedIndex,
								a6 = [],
								a7 = aZ.options,
								a2 = aZ.type === "select-one";
								if (a3 < 0) {
									return null;
								}
								for (var a0 = a2 ? a3 : 0, a4 = a2 ? a3 + 1 : a7.length; a0 < a4; a0++) {
									var a1 = a7[a0];
									if (a1.selected) {
										a5 = a(a1).val();
										if (a2) {
											return a5;
										}
										a6.push(a5);
									}
								}
								return a6;
							}
							if (I.test(aZ.type) && !a.support.checkOn) {
								return aZ.getAttribute("value") === null ? "on" : aZ.value;
							}
							return(aZ.value || "").replace(av, "");
						}
						return C;
					}
					var aY = a.isFunction(a5);
					return this.each(function (ba) {
							var a9 = a(this),
							bb = a5;
							if (this.nodeType !== 1) {
								return;
							}
							if (aY) {
								bb = a5.call(this, ba, a9.val());
							}
							if (typeof bb === "number") {
								bb += "";
							}
							if (a.isArray(bb) && I.test(this.type)) {
								this.checked = a.inArray(a9.val(), bb) >= 0;
							} else {
								if (a.nodeName(this, "select")) {
									var a8 = a.makeArray(bb);
									a("option", this).each(function () {
											this.selected = a.inArray(a(this).val(), a8) >= 0;
										});
									if (!a8.length) {
										this.selectedIndex = -1;
									}
								} else {
									this.value = bb;
								}
							}
						});
				}
			});
		a.extend({
				attrFn : {
					val : true,
					css : true,
					html : true,
					text : true,
					data : true,
					width : true,
					height : true,
					offset : true
				},
				attr : function (aZ, aY, a4, a7) {
					if (!aZ || aZ.nodeType === 3 || aZ.nodeType === 8) {
						return C;
					}
					if (a7 && aY in a.attrFn) {
						return a(aZ)[aY](a4);
					}
					var a0 = aZ.nodeType !== 1 || !a.isXMLDoc(aZ),
					a3 = a4 !== C;
					aY = a0 && a.props[aY] || aY;
					if (aZ.nodeType === 1) {
						var a2 = aQ.test(aY);
						if (aY === "selected" && !a.support.optSelected) {
							var a5 = aZ.parentNode;
							if (a5) {
								a5.selectedIndex;
								if (a5.parentNode) {
									a5.parentNode.selectedIndex;
								}
							}
						}
						if (aY in aZ && a0 && !a2) {
							if (a3) {
								if (aY === "type" && d.test(aZ.nodeName) && aZ.parentNode) {
									a.error("type property can't be changed");
								}
								aZ[aY] = a4;
							}
							if (a.nodeName(aZ, "form") && aZ.getAttributeNode(aY)) {
								return aZ.getAttributeNode(aY).nodeValue;
							}
							if (aY === "tabIndex") {
								var a6 = aZ.getAttributeNode("tabIndex");
								return a6 && a6.specified ? a6.value : z.test(aZ.nodeName) || j.test(aZ.nodeName) && aZ.href ? 0 : C;
							}
							return aZ[aY];
						}
						if (!a.support.style && a0 && aY === "style") {
							if (a3) {
								aZ.style.cssText = "" + a4;
							}
							return aZ.style.cssText;
						}
						if (a3) {
							aZ.setAttribute(aY, "" + a4);
						}
						var a1 = !a.support.hrefNormalized && a0 && a2 ? aZ.getAttribute(aY, 2) : aZ.getAttribute(aY);
						return a1 === null ? C : a1;
					}
					return a.style(aZ, aY, a4);
				}
			});
		var aC = /\.(.*)$/,
		A = function (aY) {
			return aY.replace(/[^\w\s\.\|`]/g, function (aZ) {
					return "\\" + aZ;
				});
		};
		a.event = {
			add : function (a1, a5, ba, a3) {
				if (a1.nodeType === 3 || a1.nodeType === 8) {
					return;
				}
				if (a1.setInterval && (a1 !== aM && !a1.frameElement)) {
					a1 = aM;
				}
				var aZ,
				a9;
				if (ba.handler) {
					aZ = ba;
					ba = aZ.handler;
				}
				if (!ba.guid) {
					ba.guid = a.guid++;
				}
				var a6 = a.data(a1);
				if (!a6) {
					return;
				}
				var bb = a6.events = a6.events || {
				},
				a4 = a6.handle,
				a4;
				if (!a4) {
					a6.handle = a4 = function () {
						return typeof a !== "undefined" && !a.event.triggered ? a.event.handle.apply(a4.elem, arguments) : C;
					};
				}
				a4.elem = a1;
				a5 = a5.split(" ");
				var a8,
				a2 = 0,
				aY;
				while ((a8 = a5[a2++])) {
					a9 = aZ ? a.extend({
						}, aZ) : {
						handler : ba,
						data : a3
					};
					if (a8.indexOf(".") > -1) {
						aY = a8.split(".");
						a8 = aY.shift();
						a9.namespace = aY.slice(0).sort().join(".");
					} else {
						aY = [];
						a9.namespace = "";
					}
					a9.type = a8;
					a9.guid = ba.guid;
					var a0 = bb[a8],
					a7 = a.event.special[a8] || {
					};
					if (!a0) {
						a0 = bb[a8] = [];
						if (!a7.setup || a7.setup.call(a1, a3, aY, a4) === false) {
							if (a1.addEventListener) {
								a1.addEventListener(a8, a4, false);
							} else {
								if (a1.attachEvent) {
									a1.attachEvent("on" + a8, a4);
								}
							}
						}
					}
					if (a7.add) {
						a7.add.call(a1, a9);
						if (!a9.handler.guid) {
							a9.handler.guid = ba.guid;
						}
					}
					a0.push(a9);
					a.event.global[a8] = true;
				}
				a1 = null;
			},
			global : {
			},
			remove : function (bd, a8, aZ, a4) {
				if (bd.nodeType === 3 || bd.nodeType === 8) {
					return;
				}
				var bg,
				a3,
				a5,
				bb = 0,
				a1,
				a6,
				a9,
				a2,
				a7,
				aY,
				bf,
				bc = a.data(bd),
				a0 = bc && bc.events;
				if (!bc || !a0) {
					return;
				}
				if (a8 && a8.type) {
					aZ = a8.handler;
					a8 = a8.type;
				}
				if (!a8 || typeof a8 === "string" && a8.charAt(0) === ".") {
					a8 = a8 || "";
					for (a3 in a0) {
						a.event.remove(bd, a3 + a8);
					}
					return;
				}
				a8 = a8.split(" ");
				while ((a3 = a8[bb++])) {
					bf = a3;
					aY = null;
					a1 = a3.indexOf(".") < 0;
					a6 = [];
					if (!a1) {
						a6 = a3.split(".");
						a3 = a6.shift();
						a9 = new RegExp("(^|\\.)" + a.map(a6.slice(0).sort(), A).join("\\.(?:.*\\.)?") + "(\\.|$)");
					}
					a7 = a0[a3];
					if (!a7) {
						continue;
					}
					if (!aZ) {
						for (var ba = 0; ba < a7.length; ba++) {
							aY = a7[ba];
							if (a1 || a9.test(aY.namespace)) {
								a.event.remove(bd, bf, aY.handler, ba);
								a7.splice(ba--, 1);
							}
						}
						continue;
					}
					a2 = a.event.special[a3] || {
					};
					for (var ba = a4 || 0; ba < a7.length; ba++) {
						aY = a7[ba];
						if (aZ.guid === aY.guid) {
							if (a1 || a9.test(aY.namespace)) {
								if (a4 == null) {
									a7.splice(ba--, 1);
								}
								if (a2.remove) {
									a2.remove.call(bd, aY);
								}
							}
							if (a4 != null) {
								break;
							}
						}
					}
					if (a7.length === 0 || a4 != null && a7.length === 1) {
						if (!a2.teardown || a2.teardown.call(bd, a6) === false) {
							ag(bd, a3, bc.handle);
						}
						bg = null;
						delete a0[a3];
					}
				}
				if (a.isEmptyObject(a0)) {
					var be = bc.handle;
					if (be) {
						be.elem = null;
					}
					delete bc.events;
					delete bc.handle;
					if (a.isEmptyObject(bc)) {
						a.removeData(bd);
					}
				}
			},
			trigger : function (aY, a2, a0) {
				var a7 = aY.type || aY,
				a1 = arguments[3];
				if (!a1) {
					aY = typeof aY === "object" ? aY[aI] ? aY : a.extend(a.Event(a7), aY) : a.Event(a7);
					if (a7.indexOf("!") >= 0) {
						aY.type = a7 = a7.slice(0, -1);
						aY.exclusive = true;
					}
					if (!a0) {
						aY.stopPropagation();
						if (a.event.global[a7]) {
							a.each(a.cache, function () {
									if (this.events && this.events[a7]) {
										a.event.trigger(aY, a2, this.handle.elem);
									}
								});
						}
					}
					if (!a0 || a0.nodeType === 3 || a0.nodeType === 8) {
						return C;
					}
					aY.result = C;
					aY.target = a0;
					a2 = a.makeArray(a2);
					a2.unshift(aY);
				}
				aY.currentTarget = a0;
				var a3 = a.data(a0, "handle");
				if (a3) {
					a3.apply(a0, a2);
				}
				var a8 = a0.parentNode || a0.ownerDocument;
				try {
					if (!(a0 && a0.nodeName && a.noData[a0.nodeName.toLowerCase()])) {
						if (a0["on" + a7] && a0["on" + a7].apply(a0, a2) === false) {
							aY.result = false;
						}
					}
				} catch (a5) {
				}
				if (!aY.isPropagationStopped() && a8) {
					a.event.trigger(aY, a2, a8, true);
				} else {
					if (!aY.isDefaultPrevented()) {
						var a4 = aY.target,
						aZ,
						a9 = a.nodeName(a4, "a") && a7 === "click",
						a6 = a.event.special[a7] || {
						};
						if ((!a6._default || a6._default.call(a0, aY) === false) && !a9 && !(a4 && a4.nodeName && a.noData[a4.nodeName.toLowerCase()])) {
							try {
								if (a4[a7]) {
									aZ = a4["on" + a7];
									if (aZ) {
										a4["on" + a7] = null;
									}
									a.event.triggered = true;
									a4[a7]();
								}
							} catch (a5) {
							}
							if (aZ) {
								a4["on" + a7] = aZ;
							}
							a.event.triggered = false;
						}
					}
				}
			},
			handle : function (aY) {
				var a6,
				a0,
				aZ,
				a1,
				a7;
				aY = arguments[0] = a.event.fix(aY || aM.event);
				aY.currentTarget = this;
				a6 = aY.type.indexOf(".") < 0 && !aY.exclusive;
				if (!a6) {
					aZ = aY.type.split(".");
					aY.type = aZ.shift();
					a1 = new RegExp("(^|\\.)" + aZ.slice(0).sort().join("\\.(?:.*\\.)?") + "(\\.|$)");
				}
				var a7 = a.data(this, "events"),
				a0 = a7[aY.type];
				if (a7 && a0) {
					a0 = a0.slice(0);
					for (var a3 = 0, a2 = a0.length; a3 < a2; a3++) {
						var a5 = a0[a3];
						if (a6 || a1.test(a5.namespace)) {
							aY.handler = a5.handler;
							aY.data = a5.data;
							aY.handleObj = a5;
							var a4 = a5.handler.apply(this, arguments);
							if (a4 !== C) {
								aY.result = a4;
								if (a4 === false) {
									aY.preventDefault();
									aY.stopPropagation();
								}
							}
							if (aY.isImmediatePropagationStopped()) {
								break;
							}
						}
					}
				}
				return aY.result;
			},
			props : "altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode layerX layerY metaKey newValue offsetX offsetY originalTarget pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "),
			fix : function (a1) {
				if (a1[aI]) {
					return a1;
				}
				var aZ = a1;
				a1 = a.Event(aZ);
				for (var a0 = this.props.length, a3; a0; ) {
					a3 = this.props[--a0];
					a1[a3] = aZ[a3];
				}
				if (!a1.target) {
					a1.target = a1.srcElement || ab;
				}
				if (a1.target.nodeType === 3) {
					a1.target = a1.target.parentNode;
				}
				if (!a1.relatedTarget && a1.fromElement) {
					a1.relatedTarget = a1.fromElement === a1.target ? a1.toElement : a1.fromElement;
				}
				if (a1.pageX == null && a1.clientX != null) {
					var a2 = ab.documentElement,
					aY = ab.body;
					a1.pageX = a1.clientX + (a2 && a2.scrollLeft || aY && aY.scrollLeft || 0) - (a2 && a2.clientLeft || aY && aY.clientLeft || 0);
					a1.pageY = a1.clientY + (a2 && a2.scrollTop || aY && aY.scrollTop || 0) - (a2 && a2.clientTop || aY && aY.clientTop || 0);
				}
				if (!a1.which && ((a1.charCode || a1.charCode === 0) ? a1.charCode : a1.keyCode)) {
					a1.which = a1.charCode || a1.keyCode;
				}
				if (!a1.metaKey && a1.ctrlKey) {
					a1.metaKey = a1.ctrlKey;
				}
				if (!a1.which && a1.button !== C) {
					a1.which = (a1.button & 1 ? 1 : (a1.button & 2 ? 3 : (a1.button & 4 ? 2 : 0)));
				}
				return a1;
			},
			guid : 100000000,
			proxy : a.proxy,
			special : {
				ready : {
					setup : a.bindReady,
					teardown : a.noop
				},
				live : {
					add : function (aY) {
						a.event.add(this, aY.origType, a.extend({
								}, aY, {
									handler : V
								}));
					},
					remove : function (aZ) {
						var aY = true,
						a0 = aZ.origType.replace(aC, "");
						a.each(a.data(this, "events").live || [], function () {
								if (a0 === this.origType.replace(aC, "")) {
									aY = false;
									return false;
								}
							});
						if (aY) {
							a.event.remove(this, aZ.origType, V);
						}
					}
				},
				beforeunload : {
					setup : function (a0, aZ, aY) {
						if (this.setInterval) {
							this.onbeforeunload = aY;
						}
						return false;
					},
					teardown : function (aZ, aY) {
						if (this.onbeforeunload === aY) {
							this.onbeforeunload = null;
						}
					}
				}
			}
		};
		var ag = ab.removeEventListener ? function (aZ, aY, a0) {
			aZ.removeEventListener(aY, a0, false);
		}
		 : function (aZ, aY, a0) {
			aZ.detachEvent("on" + aY, a0);
		};
		a.Event = function (aY) {
			if (!this.preventDefault) {
				return new a.Event(aY);
			}
			if (aY && aY.type) {
				this.originalEvent = aY;
				this.type = aY.type;
			} else {
				this.type = aY;
			}
			this.timeStamp = aP();
			this[aI] = true;
		};
		function aR() {
			return false;
		}
		function f() {
			return true;
		}
		a.Event.prototype = {
			preventDefault : function () {
				this.isDefaultPrevented = f;
				var aY = this.originalEvent;
				if (!aY) {
					return;
				}
				if (aY.preventDefault) {
					aY.preventDefault();
				}
				aY.returnValue = false;
			},
			stopPropagation : function () {
				this.isPropagationStopped = f;
				var aY = this.originalEvent;
				if (!aY) {
					return;
				}
				if (aY.stopPropagation) {
					aY.stopPropagation();
				}
				aY.cancelBubble = true;
			},
			stopImmediatePropagation : function () {
				this.isImmediatePropagationStopped = f;
				this.stopPropagation();
			},
			isDefaultPrevented : aR,
			isPropagationStopped : aR,
			isImmediatePropagationStopped : aR
		};
		var Q = function (aZ) {
			var aY = aZ.relatedTarget;
			try {
				while (aY && aY !== this) {
					aY = aY.parentNode;
				}
				if (aY !== this) {
					aZ.type = aZ.data;
					a.event.handle.apply(this, arguments);
				}
			} catch (a0) {
			}
		},
		ay = function (aY) {
			aY.type = aY.data;
			a.event.handle.apply(this, arguments);
		};
		a.each({
				mouseenter : "mouseover",
				mouseleave : "mouseout"
			}, function (aZ, aY) {
				a.event.special[aZ] = {
					setup : function (a0) {
						a.event.add(this, aY, a0 && a0.selector ? ay : Q, aZ);
					},
					teardown : function (a0) {
						a.event.remove(this, aY, a0 && a0.selector ? ay : Q);
					}
				};
			});
		if (!a.support.submitBubbles) {
			a.event.special.submit = {
				setup : function (aZ, aY) {
					if (this.nodeName.toLowerCase() !== "form") {
						a.event.add(this, "click.specialSubmit", function (a2) {
								var a1 = a2.target,
								a0 = a1.type;
								if ((a0 === "submit" || a0 === "image") && a(a1).closest("form").length) {
									return aA("submit", this, arguments);
								}
							});
						a.event.add(this, "keypress.specialSubmit", function (a2) {
								var a1 = a2.target,
								a0 = a1.type;
								if ((a0 === "text" || a0 === "password") && a(a1).closest("form").length && a2.keyCode === 13) {
									return aA("submit", this, arguments);
								}
							});
					} else {
						return false;
					}
				},
				teardown : function (aY) {
					a.event.remove(this, ".specialSubmit");
				}
			};
		}
		if (!a.support.changeBubbles) {
			var aq = /textarea|input|select/i,
			aS,
			i = function (aZ) {
				var aY = aZ.type,
				a0 = aZ.value;
				if (aY === "radio" || aY === "checkbox") {
					a0 = aZ.checked;
				} else {
					if (aY === "select-multiple") {
						a0 = aZ.selectedIndex > -1 ? a.map(aZ.options, function (a1) {
								return a1.selected;
							}).join("-") : "";
					} else {
						if (aZ.nodeName.toLowerCase() === "select") {
							a0 = aZ.selectedIndex;
						}
					}
				}
				return a0;
			},
			O = function O(a0) {
				var aY = a0.target,
				aZ,
				a1;
				if (!aq.test(aY.nodeName) || aY.readOnly) {
					return;
				}
				aZ = a.data(aY, "_change_data");
				a1 = i(aY);
				if (a0.type !== "focusout" || aY.type !== "radio") {
					a.data(aY, "_change_data", a1);
				}
				if (aZ === C || a1 === aZ) {
					return;
				}
				if (aZ != null || a1) {
					a0.type = "change";
					return a.event.trigger(a0, arguments[1], aY);
				}
			};
			a.event.special.change = {
				filters : {
					focusout : O,
					click : function (a0) {
						var aZ = a0.target,
						aY = aZ.type;
						if (aY === "radio" || aY === "checkbox" || aZ.nodeName.toLowerCase() === "select") {
							return O.call(this, a0);
						}
					},
					keydown : function (a0) {
						var aZ = a0.target,
						aY = aZ.type;
						if ((a0.keyCode === 13 && aZ.nodeName.toLowerCase() !== "textarea") || (a0.keyCode === 32 && (aY === "checkbox" || aY === "radio")) || aY === "select-multiple") {
							return O.call(this, a0);
						}
					},
					beforeactivate : function (aZ) {
						var aY = aZ.target;
						a.data(aY, "_change_data", i(aY));
					}
				},
				setup : function (a0, aZ) {
					if (this.type === "file") {
						return false;
					}
					for (var aY in aS) {
						a.event.add(this, aY + ".specialChange", aS[aY]);
					}
					return aq.test(this.nodeName);
				},
				teardown : function (aY) {
					a.event.remove(this, ".specialChange");
					return aq.test(this.nodeName);
				}
			};
			aS = a.event.special.change.filters;
		}
		function aA(aZ, a0, aY) {
			aY[0].type = aZ;
			return a.event.handle.apply(a0, aY);
		}
		if (ab.addEventListener) {
			a.each({
					focus : "focusin",
					blur : "focusout"
				}, function (a0, aY) {
					a.event.special[aY] = {
						setup : function () {
							this.addEventListener(a0, aZ, true);
						},
						teardown : function () {
							this.removeEventListener(a0, aZ, true);
						}
					};
					function aZ(a1) {
						a1 = a.event.fix(a1);
						a1.type = aY;
						return a.event.handle.call(this, a1);
					}
				});
		}
		a.each(["bind", "one"], function (aZ, aY) {
				a.fn[aY] = function (a5, a6, a4) {
					if (typeof a5 === "object") {
						for (var a2 in a5) {
							this[aY](a2, a6, a5[a2], a4);
						}
						return this;
					}
					if (a.isFunction(a6)) {
						a4 = a6;
						a6 = C;
					}
					var a3 = aY === "one" ? a.proxy(a4, function (a7) {
							a(this).unbind(a7, a3);
							return a4.apply(this, arguments);
						}) : a4;
					if (a5 === "unload" && aY !== "one") {
						this.one(a5, a6, a4);
					} else {
						for (var a1 = 0, a0 = this.length; a1 < a0; a1++) {
							a.event.add(this[a1], a5, a3, a6);
						}
					}
					return this;
				};
			});
		a.fn.extend({
				unbind : function (a2, a1) {
					if (typeof a2 === "object" && !a2.preventDefault) {
						for (var a0 in a2) {
							this.unbind(a0, a2[a0]);
						}
					} else {
						for (var aZ = 0, aY = this.length; aZ < aY; aZ++) {
							a.event.remove(this[aZ], a2, a1);
						}
					}
					return this;
				},
				delegate : function (aY, aZ, a1, a0) {
					return this.live(aZ, a1, a0, aY);
				},
				undelegate : function (aY, aZ, a0) {
					if (arguments.length === 0) {
						return this.unbind("live");
					} else {
						return this.die(aZ, null, a0, aY);
					}
				},
				trigger : function (aY, aZ) {
					return this.each(function () {
							a.event.trigger(aY, aZ, this);
						});
				},
				triggerHandler : function (aY, a0) {
					if (this[0]) {
						var aZ = a.Event(aY);
						aZ.preventDefault();
						aZ.stopPropagation();
						a.event.trigger(aZ, a0, this[0]);
						return aZ.result;
					}
				},
				toggle : function (a0) {
					var aY = arguments,
					aZ = 1;
					while (aZ < aY.length) {
						a.proxy(a0, aY[aZ++]);
					}
					return this.click(a.proxy(a0, function (a1) {
								var a2 = (a.data(this, "lastToggle" + a0.guid) || 0) % aZ;
								a.data(this, "lastToggle" + a0.guid, a2 + 1);
								a1.preventDefault();
								return aY[a2].apply(this, arguments) || false;
							}));
				},
				hover : function (aY, aZ) {
					return this.mouseenter(aY).mouseleave(aZ || aY);
				}
			});
		var aw = {
			focus : "focusin",
			blur : "focusout",
			mouseenter : "mouseover",
			mouseleave : "mouseout"
		};
		a.each(["live", "die"], function (aZ, aY) {
				a.fn[aY] = function (a7, a4, a9, a2) {
					var a8,
					a5 = 0,
					a6,
					a1,
					ba,
					a3 = a2 || this.selector,
					a0 = a2 ? this : a(this.context);
					if (a.isFunction(a4)) {
						a9 = a4;
						a4 = C;
					}
					a7 = (a7 || "").split(" ");
					while ((a8 = a7[a5++]) != null) {
						a6 = aC.exec(a8);
						a1 = "";
						if (a6) {
							a1 = a6[0];
							a8 = a8.replace(aC, "");
						}
						if (a8 === "hover") {
							a7.push("mouseenter" + a1, "mouseleave" + a1);
							continue;
						}
						ba = a8;
						if (a8 === "focus" || a8 === "blur") {
							a7.push(aw[a8] + a1);
							a8 = a8 + a1;
						} else {
							a8 = (aw[a8] || a8) + a1;
						}
						if (aY === "live") {
							a0.each(function () {
									a.event.add(this, m(a8, a3), {
											data : a4,
											selector : a3,
											handler : a9,
											origType : a8,
											origHandler : a9,
											preType : ba
										});
								});
						} else {
							a0.unbind(m(a8, a3), a9);
						}
					}
					return this;
				};
			});
		function V(aY) {
			var a8,
			aZ = [],
			bb = [],
			a7 = arguments,
			ba,
			a6,
			a9,
			a1,
			a3,
			a5,
			a2,
			a4,
			bc = a.data(this, "events");
			if (aY.liveFired === this || !bc || !bc.live || aY.button && aY.type === "click") {
				return;
			}
			aY.liveFired = this;
			var a0 = bc.live.slice(0);
			for (a3 = 0; a3 < a0.length; a3++) {
				a9 = a0[a3];
				if (a9.origType.replace(aC, "") === aY.type) {
					bb.push(a9.selector);
				} else {
					a0.splice(a3--, 1);
				}
			}
			a6 = a(aY.target).closest(bb, aY.currentTarget);
			for (a5 = 0, a2 = a6.length; a5 < a2; a5++) {
				for (a3 = 0; a3 < a0.length; a3++) {
					a9 = a0[a3];
					if (a6[a5].selector === a9.selector) {
						a1 = a6[a5].elem;
						ba = null;
						if (a9.preType === "mouseenter" || a9.preType === "mouseleave") {
							ba = a(aY.relatedTarget).closest(a9.selector)[0];
						}
						if (!ba || ba !== a1) {
							aZ.push({
									elem : a1,
									handleObj : a9
								});
						}
					}
				}
			}
			for (a5 = 0, a2 = aZ.length; a5 < a2; a5++) {
				a6 = aZ[a5];
				aY.currentTarget = a6.elem;
				aY.data = a6.handleObj.data;
				aY.handleObj = a6.handleObj;
				if (a6.handleObj.origHandler.apply(a6.elem, a7) === false) {
					a8 = false;
					break;
				}
			}
			return a8;
		}
		function m(aZ, aY) {
			return "live." + (aZ && aZ !== "*" ? aZ + "." : "") + aY.replace(/\./g, "`").replace(/ /g, "&");
		}
		a.each(("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error").split(" "), function (aZ, aY) {
				a.fn[aY] = function (a0) {
					return a0 ? this.bind(aY, a0) : this.trigger(aY);
				};
				if (a.attrFn) {
					a.attrFn[aY] = true;
				}
			});
		if (aM.attachEvent && !aM.addEventListener) {
			aM.attachEvent("onunload", function () {
					for (var aZ in a.cache) {
						if (a.cache[aZ].handle) {
							try {
								a.event.remove(a.cache[aZ].handle.elem);
							} catch (aY) {
							}
						}
					}
				});
			/*
			 * Sizzle CSS Selector Engine - v1.0
			 *  Copyright 2009, The Dojo Foundation
			 *  Released under the MIT, BSD, and GPL Licenses.
			 *  More information: http://sizzlejs.com/
			 */
		}
		(function () {
				var a9 = /((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^[\]]*\]|['"][^'"]*['"]|[^[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g,
				ba = 0,
				bc = Object.prototype.toString,
				a4 = false,
				a3 = true;
				[0, 0].sort(function () {
						a3 = false;
						return 0;
					});
				var a0 = function (bl, bg, bo, bp) {
					bo = bo || [];
					var br = bg = bg || ab;
					if (bg.nodeType !== 1 && bg.nodeType !== 9) {
						return[];
					}
					if (!bl || typeof bl !== "string") {
						return bo;
					}
					var bm = [],
					bi,
					bt,
					bw,
					bh,
					bk = true,
					bj = a1(bg),
					bq = bl;
					while ((a9.exec(""), bi = a9.exec(bq)) !== null) {
						bq = bi[3];
						bm.push(bi[1]);
						if (bi[2]) {
							bh = bi[3];
							break;
						}
					}
					if (bm.length > 1 && a5.exec(bl)) {
						if (bm.length === 2 && a6.relative[bm[0]]) {
							bt = bd(bm[0] + bm[1], bg);
						} else {
							bt = a6.relative[bm[0]] ? [bg] : a0(bm.shift(), bg);
							while (bm.length) {
								bl = bm.shift();
								if (a6.relative[bl]) {
									bl += bm.shift();
								}
								bt = bd(bl, bt);
							}
						}
					} else {
						if (!bp && bm.length > 1 && bg.nodeType === 9 && !bj && a6.match.ID.test(bm[0]) && !a6.match.ID.test(bm[bm.length - 1])) {
							var bs = a0.find(bm.shift(), bg, bj);
							bg = bs.expr ? a0.filter(bs.expr, bs.set)[0] : bs.set[0];
						}
						if (bg) {
							var bs = bp ? {
								expr : bm.pop(),
								set : a8(bp)
							}
							 : a0.find(bm.pop(), bm.length === 1 && (bm[0] === "~" || bm[0] === "+") && bg.parentNode ? bg.parentNode : bg, bj);
							bt = bs.expr ? a0.filter(bs.expr, bs.set) : bs.set;
							if (bm.length > 0) {
								bw = a8(bt);
							} else {
								bk = false;
							}
							while (bm.length) {
								var bv = bm.pop(),
								bu = bv;
								if (!a6.relative[bv]) {
									bv = "";
								} else {
									bu = bm.pop();
								}
								if (bu == null) {
									bu = bg;
								}
								a6.relative[bv](bw, bu, bj);
							}
						} else {
							bw = bm = [];
						}
					}
					if (!bw) {
						bw = bt;
					}
					if (!bw) {
						a0.error(bv || bl);
					}
					if (bc.call(bw) === "[object Array]") {
						if (!bk) {
							bo.push.apply(bo, bw);
						} else {
							if (bg && bg.nodeType === 1) {
								for (var bn = 0; bw[bn] != null; bn++) {
									if (bw[bn] && (bw[bn] === true || bw[bn].nodeType === 1 && a7(bg, bw[bn]))) {
										bo.push(bt[bn]);
									}
								}
							} else {
								for (var bn = 0; bw[bn] != null; bn++) {
									if (bw[bn] && bw[bn].nodeType === 1) {
										bo.push(bt[bn]);
									}
								}
							}
						}
					} else {
						a8(bw, bo);
					}
					if (bh) {
						a0(bh, br, bo, bp);
						a0.uniqueSort(bo);
					}
					return bo;
				};
				a0.uniqueSort = function (bh) {
					if (bb) {
						a4 = a3;
						bh.sort(bb);
						if (a4) {
							for (var bg = 1; bg < bh.length; bg++) {
								if (bh[bg] === bh[bg - 1]) {
									bh.splice(bg--, 1);
								}
							}
						}
					}
					return bh;
				};
				a0.matches = function (bg, bh) {
					return a0(bg, null, null, bh);
				};
				a0.find = function (bn, bg, bo) {
					var bm,
					bk;
					if (!bn) {
						return[];
					}
					for (var bj = 0, bi = a6.order.length; bj < bi; bj++) {
						var bl = a6.order[bj],
						bk;
						if ((bk = a6.leftMatch[bl].exec(bn))) {
							var bh = bk[1];
							bk.splice(1, 1);
							if (bh.substr(bh.length - 1) !== "\\") {
								bk[1] = (bk[1] || "").replace(/\\/g, "");
								bm = a6.find[bl](bk, bg, bo);
								if (bm != null) {
									bn = bn.replace(a6.match[bl], "");
									break;
								}
							}
						}
					}
					if (!bm) {
						bm = bg.getElementsByTagName("*");
					}
					return{
						set : bm,
						expr : bn
					};
				};
				a0.filter = function (br, bq, bu, bk) {
					var bi = br,
					bw = [],
					bo = bq,
					bm,
					bg,
					bn = bq && bq[0] && a1(bq[0]);
					while (br && bq.length) {
						for (var bp in a6.filter) {
							if ((bm = a6.leftMatch[bp].exec(br)) != null && bm[2]) {
								var bh = a6.filter[bp],
								bv,
								bt,
								bj = bm[1];
								bg = false;
								bm.splice(1, 1);
								if (bj.substr(bj.length - 1) === "\\") {
									continue;
								}
								if (bo === bw) {
									bw = [];
								}
								if (a6.preFilter[bp]) {
									bm = a6.preFilter[bp](bm, bo, bu, bw, bk, bn);
									if (!bm) {
										bg = bv = true;
									} else {
										if (bm === true) {
											continue;
										}
									}
								}
								if (bm) {
									for (var bl = 0; (bt = bo[bl]) != null; bl++) {
										if (bt) {
											bv = bh(bt, bm, bl, bo);
											var bs = bk^!!bv;
											if (bu && bv != null) {
												if (bs) {
													bg = true;
												} else {
													bo[bl] = false;
												}
											} else {
												if (bs) {
													bw.push(bt);
													bg = true;
												}
											}
										}
									}
								}
								if (bv !== C) {
									if (!bu) {
										bo = bw;
									}
									br = br.replace(a6.match[bp], "");
									if (!bg) {
										return[];
									}
									break;
								}
							}
						}
						if (br === bi) {
							if (bg == null) {
								a0.error(br);
							} else {
								break;
							}
						}
						bi = br;
					}
					return bo;
				};
				a0.error = function (bg) {
					throw "Syntax error, unrecognized expression: " + bg;
				};
				var a6 = a0.selectors = {
					order : ["ID", "NAME", "TAG"],
					match : {
						ID : /#((?:[\w\u00c0-\uFFFF-]|\\.)+)/,
						CLASS : /\.((?:[\w\u00c0-\uFFFF-]|\\.)+)/,
						NAME : /\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\]/,
						ATTR : /\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\]/,
						TAG : /^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)/,
						CHILD : /:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?/,
						POS : /:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)/,
						PSEUDO : /:((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/
					},
					leftMatch : {
					},
					attrMap : {
						"class" : "className",
						"for" : "htmlFor"
					},
					attrHandle : {
						href : function (bg) {
							return bg.getAttribute("href");
						}
					},
					relative : {
						"+" : function (bm, bh) {
							var bj = typeof bh === "string",
							bl = bj && !/\W/.test(bh),
							bn = bj && !bl;
							if (bl) {
								bh = bh.toLowerCase();
							}
							for (var bi = 0, bg = bm.length, bk; bi < bg; bi++) {
								if ((bk = bm[bi])) {
									while ((bk = bk.previousSibling) && bk.nodeType !== 1) {
									}
									bm[bi] = bn || bk && bk.nodeName.toLowerCase() === bh ? bk || false : bk === bh;
								}
							}
							if (bn) {
								a0.filter(bh, bm, true);
							}
						},
						">" : function (bm, bh) {
							var bk = typeof bh === "string";
							if (bk && !/\W/.test(bh)) {
								bh = bh.toLowerCase();
								for (var bi = 0, bg = bm.length; bi < bg; bi++) {
									var bl = bm[bi];
									if (bl) {
										var bj = bl.parentNode;
										bm[bi] = bj.nodeName.toLowerCase() === bh ? bj : false;
									}
								}
							} else {
								for (var bi = 0, bg = bm.length; bi < bg; bi++) {
									var bl = bm[bi];
									if (bl) {
										bm[bi] = bk ? bl.parentNode : bl.parentNode === bh;
									}
								}
								if (bk) {
									a0.filter(bh, bm, true);
								}
							}
						},
						"" : function (bj, bh, bl) {
							var bi = ba++,
							bg = be;
							if (typeof bh === "string" && !/\W/.test(bh)) {
								var bk = bh = bh.toLowerCase();
								bg = aY;
							}
							bg("parentNode", bh, bi, bj, bk, bl);
						},
						"~" : function (bj, bh, bl) {
							var bi = ba++,
							bg = be;
							if (typeof bh === "string" && !/\W/.test(bh)) {
								var bk = bh = bh.toLowerCase();
								bg = aY;
							}
							bg("previousSibling", bh, bi, bj, bk, bl);
						}
					},
					find : {
						ID : function (bh, bi, bj) {
							if (typeof bi.getElementById !== "undefined" && !bj) {
								var bg = bi.getElementById(bh[1]);
								return bg ? [bg] : [];
							}
						},
						NAME : function (bi, bl) {
							if (typeof bl.getElementsByName !== "undefined") {
								var bh = [],
								bk = bl.getElementsByName(bi[1]);
								for (var bj = 0, bg = bk.length; bj < bg; bj++) {
									if (bk[bj].getAttribute("name") === bi[1]) {
										bh.push(bk[bj]);
									}
								}
								return bh.length === 0 ? null : bh;
							}
						},
						TAG : function (bg, bh) {
							return bh.getElementsByTagName(bg[1]);
						}
					},
					preFilter : {
						CLASS : function (bj, bh, bi, bg, bm, bn) {
							bj = " " + bj[1].replace(/\\/g, "") + " ";
							if (bn) {
								return bj;
							}
							for (var bk = 0, bl; (bl = bh[bk]) != null; bk++) {
								if (bl) {
									if (bm^(bl.className && (" " + bl.className + " ").replace(/[\t\n]/g, " ").indexOf(bj) >= 0)) {
										if (!bi) {
											bg.push(bl);
										}
									} else {
										if (bi) {
											bh[bk] = false;
										}
									}
								}
							}
							return false;
						},
						ID : function (bg) {
							return bg[1].replace(/\\/g, "");
						},
						TAG : function (bh, bg) {
							return bh[1].toLowerCase();
						},
						CHILD : function (bg) {
							if (bg[1] === "nth") {
								var bh = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(bg[2] === "even" && "2n" || bg[2] === "odd" && "2n+1" || !/\D/.test(bg[2]) && "0n+" + bg[2] || bg[2]);
								bg[2] = (bh[1] + (bh[2] || 1)) - 0;
								bg[3] = bh[3] - 0;
							}
							bg[0] = ba++;
							return bg;
						},
						ATTR : function (bk, bh, bi, bg, bl, bm) {
							var bj = bk[1].replace(/\\/g, "");
							if (!bm && a6.attrMap[bj]) {
								bk[1] = a6.attrMap[bj];
							}
							if (bk[2] === "~=") {
								bk[4] = " " + bk[4] + " ";
							}
							return bk;
						},
						PSEUDO : function (bk, bh, bi, bg, bl) {
							if (bk[1] === "not") {
								if ((a9.exec(bk[3]) || "").length > 1 || /^\w/.test(bk[3])) {
									bk[3] = a0(bk[3], null, null, bh);
								} else {
									var bj = a0.filter(bk[3], bh, bi, true^bl);
									if (!bi) {
										bg.push.apply(bg, bj);
									}
									return false;
								}
							} else {
								if (a6.match.POS.test(bk[0]) || a6.match.CHILD.test(bk[0])) {
									return true;
								}
							}
							return bk;
						},
						POS : function (bg) {
							bg.unshift(true);
							return bg;
						}
					},
					filters : {
						enabled : function (bg) {
							return bg.disabled === false && bg.type !== "hidden";
						},
						disabled : function (bg) {
							return bg.disabled === true;
						},
						checked : function (bg) {
							return bg.checked === true;
						},
						selected : function (bg) {
							bg.parentNode.selectedIndex;
							return bg.selected === true;
						},
						parent : function (bg) {
							return!!bg.firstChild;
						},
						empty : function (bg) {
							return!bg.firstChild;
						},
						has : function (bi, bh, bg) {
							return!!a0(bg[3], bi).length;
						},
						header : function (bg) {
							return/h\d/i.test(bg.nodeName);
						},
						text : function (bg) {
							return "text" === bg.type;
						},
						radio : function (bg) {
							return "radio" === bg.type;
						},
						checkbox : function (bg) {
							return "checkbox" === bg.type;
						},
						file : function (bg) {
							return "file" === bg.type;
						},
						password : function (bg) {
							return "password" === bg.type;
						},
						submit : function (bg) {
							return "submit" === bg.type;
						},
						image : function (bg) {
							return "image" === bg.type;
						},
						reset : function (bg) {
							return "reset" === bg.type;
						},
						button : function (bg) {
							return "button" === bg.type || bg.nodeName.toLowerCase() === "button";
						},
						input : function (bg) {
							return/input|select|textarea|button/i.test(bg.nodeName);
						}
					},
					setFilters : {
						first : function (bh, bg) {
							return bg === 0;
						},
						last : function (bi, bh, bg, bj) {
							return bh === bj.length - 1;
						},
						even : function (bh, bg) {
							return bg % 2 === 0;
						},
						odd : function (bh, bg) {
							return bg % 2 === 1;
						},
						lt : function (bi, bh, bg) {
							return bh < bg[3] - 0;
						},
						gt : function (bi, bh, bg) {
							return bh > bg[3] - 0;
						},
						nth : function (bi, bh, bg) {
							return bg[3] - 0 === bh;
						},
						eq : function (bi, bh, bg) {
							return bg[3] - 0 === bh;
						}
					},
					filter : {
						PSEUDO : function (bm, bi, bj, bn) {
							var bh = bi[1],
							bk = a6.filters[bh];
							if (bk) {
								return bk(bm, bj, bi, bn);
							} else {
								if (bh === "contains") {
									return(bm.textContent || bm.innerText || aZ([bm]) || "").indexOf(bi[3]) >= 0;
								} else {
									if (bh === "not") {
										var bl = bi[3];
										for (var bj = 0, bg = bl.length; bj < bg; bj++) {
											if (bl[bj] === bm) {
												return false;
											}
										}
										return true;
									} else {
										a0.error("Syntax error, unrecognized expression: " + bh);
									}
								}
							}
						},
						CHILD : function (bg, bj) {
							var bm = bj[1],
							bh = bg;
							switch (bm) {
							case "only": 
							case "first": 
								while ((bh = bh.previousSibling)) {
									if (bh.nodeType === 1) {
										return false;
									}
								}
								if (bm === "first") {
									return true;
								}
								bh = bg;
							case "last": 
								while ((bh = bh.nextSibling)) {
									if (bh.nodeType === 1) {
										return false;
									}
								}
								return true;
							case "nth": 
								var bi = bj[2],
								bp = bj[3];
								if (bi === 1 && bp === 0) {
									return true;
								}
								var bl = bj[0],
								bo = bg.parentNode;
								if (bo && (bo.sizcache !== bl || !bg.nodeIndex)) {
									var bk = 0;
									for (bh = bo.firstChild; bh; bh = bh.nextSibling) {
										if (bh.nodeType === 1) {
											bh.nodeIndex = ++bk;
										}
									}
									bo.sizcache = bl;
								}
								var bn = bg.nodeIndex - bp;
								if (bi === 0) {
									return bn === 0;
								} else {
									return(bn % bi === 0 && bn / bi >= 0);
								}
							}
						},
						ID : function (bh, bg) {
							return bh.nodeType === 1 && bh.getAttribute("id") === bg;
						},
						TAG : function (bh, bg) {
							return(bg === "*" && bh.nodeType === 1) || bh.nodeName.toLowerCase() === bg;
						},
						CLASS : function (bh, bg) {
							return(" " + (bh.className || bh.getAttribute("class")) + " ").indexOf(bg) > -1;
						},
						ATTR : function (bl, bj) {
							var bi = bj[1],
							bg = a6.attrHandle[bi] ? a6.attrHandle[bi](bl) : bl[bi] != null ? bl[bi] : bl.getAttribute(bi),
							bm = bg + "",
							bk = bj[2],
							bh = bj[4];
							return bg == null ? bk === "!=" : bk === "=" ? bm === bh : bk === "*=" ? bm.indexOf(bh) >= 0 : bk === "~=" ? (" " + bm + " ").indexOf(bh) >= 0 : !bh ? bm && bg !== false : bk === "!=" ? bm !== bh : bk === "^=" ? bm.indexOf(bh) === 0 : bk === "$=" ? bm.substr(bm.length - bh.length) === bh : bk === "|=" ? bm === bh || bm.substr(0, bh.length + 1) === bh + "-" : false;
						},
						POS : function (bk, bh, bi, bl) {
							var bg = bh[2],
							bj = a6.setFilters[bg];
							if (bj) {
								return bj(bk, bi, bh, bl);
							}
						}
					}
				};
				var a5 = a6.match.POS;
				for (var a2 in a6.match) {
					a6.match[a2] = new RegExp(a6.match[a2].source + /(?![^\[]*\])(?![^\(]*\))/.source);
					a6.leftMatch[a2] = new RegExp(/(^(?:.|\r|\n)*?)/.source + a6.match[a2].source.replace(/\\(\d+)/g, function (bh, bg) {
								return "\\" + (bg - 0 + 1);
							}));
				}
				var a8 = function (bh, bg) {
					bh = Array.prototype.slice.call(bh, 0);
					if (bg) {
						bg.push.apply(bg, bh);
						return bg;
					}
					return bh;
				};
				try {
					Array.prototype.slice.call(ab.documentElement.childNodes, 0)[0].nodeType;
				} catch (bf) {
					a8 = function (bk, bj) {
						var bh = bj || [];
						if (bc.call(bk) === "[object Array]") {
							Array.prototype.push.apply(bh, bk);
						} else {
							if (typeof bk.length === "number") {
								for (var bi = 0, bg = bk.length; bi < bg; bi++) {
									bh.push(bk[bi]);
								}
							} else {
								for (var bi = 0; bk[bi]; bi++) {
									bh.push(bk[bi]);
								}
							}
						}
						return bh;
					};
				}
				var bb;
				if (ab.documentElement.compareDocumentPosition) {
					bb = function (bh, bg) {
						if (!bh.compareDocumentPosition || !bg.compareDocumentPosition) {
							if (bh == bg) {
								a4 = true;
							}
							return bh.compareDocumentPosition ? -1 : 1;
						}
						var bi = bh.compareDocumentPosition(bg) & 4 ? -1 : bh === bg ? 0 : 1;
						if (bi === 0) {
							a4 = true;
						}
						return bi;
					};
				} else {
					if ("sourceIndex" in ab.documentElement) {
						bb = function (bh, bg) {
							if (!bh.sourceIndex || !bg.sourceIndex) {
								if (bh == bg) {
									a4 = true;
								}
								return bh.sourceIndex ? -1 : 1;
							}
							var bi = bh.sourceIndex - bg.sourceIndex;
							if (bi === 0) {
								a4 = true;
							}
							return bi;
						};
					} else {
						if (ab.createRange) {
							bb = function (bj, bh) {
								if (!bj.ownerDocument || !bh.ownerDocument) {
									if (bj == bh) {
										a4 = true;
									}
									return bj.ownerDocument ? -1 : 1;
								}
								var bi = bj.ownerDocument.createRange(),
								bg = bh.ownerDocument.createRange();
								bi.setStart(bj, 0);
								bi.setEnd(bj, 0);
								bg.setStart(bh, 0);
								bg.setEnd(bh, 0);
								var bk = bi.compareBoundaryPoints(Range.START_TO_END, bg);
								if (bk === 0) {
									a4 = true;
								}
								return bk;
							};
						}
					}
				}
				function aZ(bg) {
					var bh = "",
					bj;
					for (var bi = 0; bg[bi]; bi++) {
						bj = bg[bi];
						if (bj.nodeType === 3 || bj.nodeType === 4) {
							bh += bj.nodeValue;
						} else {
							if (bj.nodeType !== 8) {
								bh += aZ(bj.childNodes);
							}
						}
					}
					return bh;
				}
				(function () {
						var bh = ab.createElement("div"),
						bi = "script" + (new Date).getTime();
						bh.innerHTML = "<a name='" + bi + "'/>";
						var bg = ab.documentElement;
						bg.insertBefore(bh, bg.firstChild);
						if (ab.getElementById(bi)) {
							a6.find.ID = function (bk, bl, bm) {
								if (typeof bl.getElementById !== "undefined" && !bm) {
									var bj = bl.getElementById(bk[1]);
									return bj ? bj.id === bk[1] || typeof bj.getAttributeNode !== "undefined" && bj.getAttributeNode("id").nodeValue === bk[1] ? [bj] : C : [];
								}
							};
							a6.filter.ID = function (bl, bj) {
								var bk = typeof bl.getAttributeNode !== "undefined" && bl.getAttributeNode("id");
								return bl.nodeType === 1 && bk && bk.nodeValue === bj;
							};
						}
						bg.removeChild(bh);
						bg = bh = null;
					})();
				(function () {
						var bg = ab.createElement("div");
						bg.appendChild(ab.createComment(""));
						if (bg.getElementsByTagName("*").length > 0) {
							a6.find.TAG = function (bh, bl) {
								var bk = bl.getElementsByTagName(bh[1]);
								if (bh[1] === "*") {
									var bj = [];
									for (var bi = 0; bk[bi]; bi++) {
										if (bk[bi].nodeType === 1) {
											bj.push(bk[bi]);
										}
									}
									bk = bj;
								}
								return bk;
							};
						}
						bg.innerHTML = "<a href='#'></a>";
						if (bg.firstChild && typeof bg.firstChild.getAttribute !== "undefined" && bg.firstChild.getAttribute("href") !== "#") {
							a6.attrHandle.href = function (bh) {
								return bh.getAttribute("href", 2);
							};
						}
						bg = null;
					})();
				if (ab.querySelectorAll) {
					(function () {
							var bg = a0,
							bi = ab.createElement("div");
							bi.innerHTML = "<p class='TEST'></p>";
							if (bi.querySelectorAll && bi.querySelectorAll(".TEST").length === 0) {
								return;
							}
							a0 = function (bm, bl, bj, bk) {
								bl = bl || ab;
								if (!bk && bl.nodeType === 9 && !a1(bl)) {
									try {
										return a8(bl.querySelectorAll(bm), bj);
									} catch (bn) {
									}
								}
								return bg(bm, bl, bj, bk);
							};
							for (var bh in bg) {
								a0[bh] = bg[bh];
							}
							bi = null;
						})();
				}
				(function () {
						var bg = ab.createElement("div");
						bg.innerHTML = "<div class='test e'></div><div class='test'></div>";
						if (!bg.getElementsByClassName || bg.getElementsByClassName("e").length === 0) {
							return;
						}
						bg.lastChild.className = "e";
						if (bg.getElementsByClassName("e").length === 1) {
							return;
						}
						a6.order.splice(1, 0, "CLASS");
						a6.find.CLASS = function (bh, bi, bj) {
							if (typeof bi.getElementsByClassName !== "undefined" && !bj) {
								return bi.getElementsByClassName(bh[1]);
							}
						};
						bg = null;
					})();
				function aY(bh, bm, bl, bp, bn, bo) {
					for (var bj = 0, bi = bp.length; bj < bi; bj++) {
						var bg = bp[bj];
						if (bg) {
							bg = bg[bh];
							var bk = false;
							while (bg) {
								if (bg.sizcache === bl) {
									bk = bp[bg.sizset];
									break;
								}
								if (bg.nodeType === 1 && !bo) {
									bg.sizcache = bl;
									bg.sizset = bj;
								}
								if (bg.nodeName.toLowerCase() === bm) {
									bk = bg;
									break;
								}
								bg = bg[bh];
							}
							bp[bj] = bk;
						}
					}
				}
				function be(bh, bm, bl, bp, bn, bo) {
					for (var bj = 0, bi = bp.length; bj < bi; bj++) {
						var bg = bp[bj];
						if (bg) {
							bg = bg[bh];
							var bk = false;
							while (bg) {
								if (bg.sizcache === bl) {
									bk = bp[bg.sizset];
									break;
								}
								if (bg.nodeType === 1) {
									if (!bo) {
										bg.sizcache = bl;
										bg.sizset = bj;
									}
									if (typeof bm !== "string") {
										if (bg === bm) {
											bk = true;
											break;
										}
									} else {
										if (a0.filter(bm, [bg]).length > 0) {
											bk = bg;
											break;
										}
									}
								}
								bg = bg[bh];
							}
							bp[bj] = bk;
						}
					}
				}
				var a7 = ab.compareDocumentPosition ? function (bh, bg) {
					return!!(bh.compareDocumentPosition(bg) & 16);
				}
				 : function (bh, bg) {
					return bh !== bg && (bh.contains ? bh.contains(bg) : true);
				};
				var a1 = function (bg) {
					var bh = (bg ? bg.ownerDocument || bg : 0).documentElement;
					return bh ? bh.nodeName !== "HTML" : false;
				};
				var bd = function (bg, bn) {
					var bj = [],
					bk = "",
					bl,
					bi = bn.nodeType ? [bn] : bn;
					while ((bl = a6.match.PSEUDO.exec(bg))) {
						bk += bl[0];
						bg = bg.replace(a6.match.PSEUDO, "");
					}
					bg = a6.relative[bg] ? bg + "*" : bg;
					for (var bm = 0, bh = bi.length; bm < bh; bm++) {
						a0(bg, bi[bm], bj);
					}
					return a0.filter(bk, bj);
				};
				a.find = a0;
				a.expr = a0.selectors;
				a.expr[":"] = a.expr.filters;
				a.unique = a0.uniqueSort;
				a.text = aZ;
				a.isXMLDoc = a1;
				a.contains = a7;
				return;
				aM.Sizzle = a0;
			})();
		var N = /Until$/,
		Y = /^(?:parents|prevUntil|prevAll)/,
		aL = /,/,
		F = Array.prototype.slice;
		var ai = function (a1, a0, aY) {
			if (a.isFunction(a0)) {
				return a.grep(a1, function (a3, a2) {
						return!!a0.call(a3, a2, a3) === aY;
					});
			} else {
				if (a0.nodeType) {
					return a.grep(a1, function (a3, a2) {
							return(a3 === a0) === aY;
						});
				} else {
					if (typeof a0 === "string") {
						var aZ = a.grep(a1, function (a2) {
								return a2.nodeType === 1;
							});
						if (aW.test(a0)) {
							return a.filter(a0, aZ, !aY);
						} else {
							a0 = a.filter(a0, aZ);
						}
					}
				}
			}
			return a.grep(a1, function (a3, a2) {
					return(a.inArray(a3, a0) >= 0) === aY;
				});
		};
		a.fn.extend({
				find : function (aY) {
					var a0 = this.pushStack("", "find", aY),
					a3 = 0;
					for (var a1 = 0, aZ = this.length; a1 < aZ; a1++) {
						a3 = a0.length;
						a.find(aY, this[a1], a0);
						if (a1 > 0) {
							for (var a4 = a3; a4 < a0.length; a4++) {
								for (var a2 = 0; a2 < a3; a2++) {
									if (a0[a2] === a0[a4]) {
										a0.splice(a4--, 1);
										break;
									}
								}
							}
						}
					}
					return a0;
				},
				has : function (aZ) {
					var aY = a(aZ);
					return this.filter(function () {
							for (var a1 = 0, a0 = aY.length; a1 < a0; a1++) {
								if (a.contains(this, aY[a1])) {
									return true;
								}
							}
						});
				},
				not : function (aY) {
					return this.pushStack(ai(this, aY, false), "not", aY);
				},
				filter : function (aY) {
					return this.pushStack(ai(this, aY, true), "filter", aY);
				},
				is : function (aY) {
					return!!aY && a.filter(aY, this).length > 0;
				},
				closest : function (a7, aY) {
					if (a.isArray(a7)) {
						var a4 = [],
						a6 = this[0],
						a3,
						a2 = {
						},
						a0;
						if (a6 && a7.length) {
							for (var a1 = 0, aZ = a7.length; a1 < aZ; a1++) {
								a0 = a7[a1];
								if (!a2[a0]) {
									a2[a0] = a.expr.match.POS.test(a0) ? a(a0, aY || this.context) : a0;
								}
							}
							while (a6 && a6.ownerDocument && a6 !== aY) {
								for (a0 in a2) {
									a3 = a2[a0];
									if (a3.jquery ? a3.index(a6) > -1 : a(a6).is(a3)) {
										a4.push({
												selector : a0,
												elem : a6
											});
										delete a2[a0];
									}
								}
								a6 = a6.parentNode;
							}
						}
						return a4;
					}
					var a5 = a.expr.match.POS.test(a7) ? a(a7, aY || this.context) : null;
					return this.map(function (a8, a9) {
							while (a9 && a9.ownerDocument && a9 !== aY) {
								if (a5 ? a5.index(a9) > -1 : a(a9).is(a7)) {
									return a9;
								}
								a9 = a9.parentNode;
							}
							return null;
						});
				},
				index : function (aY) {
					if (!aY || typeof aY === "string") {
						return a.inArray(this[0], aY ? a(aY) : this.parent().children());
					}
					return a.inArray(aY.jquery ? aY[0] : aY, this);
				},
				add : function (aY, aZ) {
					var a1 = typeof aY === "string" ? a(aY, aZ || this.context) : a.makeArray(aY),
					a0 = a.merge(this.get(), a1);
					return this.pushStack(y(a1[0]) || y(a0[0]) ? a0 : a.unique(a0));
				},
				andSelf : function () {
					return this.add(this.prevObject);
				}
			});
		function y(aY) {
			return!aY || !aY.parentNode || aY.parentNode.nodeType === 11;
		}
		a.each({
				parent : function (aZ) {
					var aY = aZ.parentNode;
					return aY && aY.nodeType !== 11 ? aY : null;
				},
				parents : function (aY) {
					return a.dir(aY, "parentNode");
				},
				parentsUntil : function (aZ, aY, a0) {
					return a.dir(aZ, "parentNode", a0);
				},
				next : function (aY) {
					return a.nth(aY, 2, "nextSibling");
				},
				prev : function (aY) {
					return a.nth(aY, 2, "previousSibling");
				},
				nextAll : function (aY) {
					return a.dir(aY, "nextSibling");
				},
				prevAll : function (aY) {
					return a.dir(aY, "previousSibling");
				},
				nextUntil : function (aZ, aY, a0) {
					return a.dir(aZ, "nextSibling", a0);
				},
				prevUntil : function (aZ, aY, a0) {
					return a.dir(aZ, "previousSibling", a0);
				},
				siblings : function (aY) {
					return a.sibling(aY.parentNode.firstChild, aY);
				},
				children : function (aY) {
					return a.sibling(aY.firstChild);
				},
				contents : function (aY) {
					return a.nodeName(aY, "iframe") ? aY.contentDocument || aY.contentWindow.document : a.makeArray(aY.childNodes);
				}
			}, function (aY, aZ) {
				a.fn[aY] = function (a2, a0) {
					var a1 = a.map(this, aZ, a2);
					if (!N.test(aY)) {
						a0 = a2;
					}
					if (a0 && typeof a0 === "string") {
						a1 = a.filter(a0, a1);
					}
					a1 = this.length > 1 ? a.unique(a1) : a1;
					if ((this.length > 1 || aL.test(a0)) && Y.test(aY)) {
						a1 = a1.reverse();
					}
					return this.pushStack(a1, aY, F.call(arguments).join(","));
				};
			});
		a.extend({
				filter : function (a0, aY, aZ) {
					if (aZ) {
						a0 = ":not(" + a0 + ")";
					}
					return a.find.matches(a0, aY);
				},
				dir : function (a0, aZ, a2) {
					var aY = [],
					a1 = a0[aZ];
					while (a1 && a1.nodeType !== 9 && (a2 === C || a1.nodeType !== 1 || !a(a1).is(a2))) {
						if (a1.nodeType === 1) {
							aY.push(a1);
						}
						a1 = a1[aZ];
					}
					return aY;
				},
				nth : function (a2, aY, a0, a1) {
					aY = aY || 1;
					var aZ = 0;
					for (; a2; a2 = a2[a0]) {
						if (a2.nodeType === 1 && ++aZ === aY) {
							break;
						}
					}
					return a2;
				},
				sibling : function (a0, aZ) {
					var aY = [];
					for (; a0; a0 = a0.nextSibling) {
						if (a0.nodeType === 1 && a0 !== aZ) {
							aY.push(a0);
						}
					}
					return aY;
				}
			});
		var T = / jQuery\d+="(?:\d+|null)"/g,
		Z = /^\s+/,
		H = /(<([\w:]+)[^>]*?)\/>/g,
		al = /^(?:area|br|col|embed|hr|img|input|link|meta|param)$/i,
		c = /<([\w:]+)/,
		t = /<tbody/i,
		L = /<|&#?\w+;/,
		E = /<script|<object|<embed|<option|<style/i,
		l = /checked\s*(?:[^=]|=\s*.checked.)/i,
		p = function (aZ, a0, aY) {
			return al.test(aY) ? aZ : a0 + "></" + aY + ">";
		},
		ac = {
			option : [1, "<select multiple='multiple'>", "</select>"],
			legend : [1, "<fieldset>", "</fieldset>"],
			thead : [1, "<table>", "</table>"],
			tr : [2, "<table><tbody>", "</tbody></table>"],
			td : [3, "<table><tbody><tr>", "</tr></tbody></table>"],
			col : [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"],
			area : [1, "<map>", "</map>"],
			_default : [0, "", ""]
		};
		ac.optgroup = ac.option;
		ac.tbody = ac.tfoot = ac.colgroup = ac.caption = ac.thead;
		ac.th = ac.td;
		if (!a.support.htmlSerialize) {
			ac._default = [1, "div<div>", "</div>"];
		}
		a.fn.extend({
				text : function (aY) {
					if (a.isFunction(aY)) {
						return this.each(function (a0) {
								var aZ = a(this);
								aZ.text(aY.call(this, a0, aZ.text()));
							});
					}
					if (typeof aY !== "object" && aY !== C) {
						return this.empty().append((this[0] && this[0].ownerDocument || ab).createTextNode(aY));
					}
					return a.text(this);
				},
				wrapAll : function (aY) {
					if (a.isFunction(aY)) {
						return this.each(function (a0) {
								a(this).wrapAll(aY.call(this, a0));
							});
					}
					if (this[0]) {
						var aZ = a(aY, this[0].ownerDocument).eq(0).clone(true);
						if (this[0].parentNode) {
							aZ.insertBefore(this[0]);
						}
						aZ.map(function () {
								var a0 = this;
								while (a0.firstChild && a0.firstChild.nodeType === 1) {
									a0 = a0.firstChild;
								}
								return a0;
							}).append(this);
					}
					return this;
				},
				wrapInner : function (aY) {
					if (a.isFunction(aY)) {
						return this.each(function (aZ) {
								a(this).wrapInner(aY.call(this, aZ));
							});
					}
					return this.each(function () {
							var aZ = a(this),
							a0 = aZ.contents();
							if (a0.length) {
								a0.wrapAll(aY);
							} else {
								aZ.append(aY);
							}
						});
				},
				wrap : function (aY) {
					return this.each(function () {
							a(this).wrapAll(aY);
						});
				},
				unwrap : function () {
					return this.parent().each(function () {
							if (!a.nodeName(this, "body")) {
								a(this).replaceWith(this.childNodes);
							}
						}).end();
				},
				append : function () {
					return this.domManip(arguments, true, function (aY) {
							if (this.nodeType === 1) {
								this.appendChild(aY);
							}
						});
				},
				prepend : function () {
					return this.domManip(arguments, true, function (aY) {
							if (this.nodeType === 1) {
								this.insertBefore(aY, this.firstChild);
							}
						});
				},
				before : function () {
					if (this[0] && this[0].parentNode) {
						return this.domManip(arguments, false, function (aZ) {
								this.parentNode.insertBefore(aZ, this);
							});
					} else {
						if (arguments.length) {
							var aY = a(arguments[0]);
							aY.push.apply(aY, this.toArray());
							return this.pushStack(aY, "before", arguments);
						}
					}
				},
				after : function () {
					if (this[0] && this[0].parentNode) {
						return this.domManip(arguments, false, function (aZ) {
								this.parentNode.insertBefore(aZ, this.nextSibling);
							});
					} else {
						if (arguments.length) {
							var aY = this.pushStack(this, "after", arguments);
							aY.push.apply(aY, a(arguments[0]).toArray());
							return aY;
						}
					}
				},
				remove : function (aY, a1) {
					for (var aZ = 0, a0; (a0 = this[aZ]) != null; aZ++) {
						if (!aY || a.filter(aY, [a0]).length) {
							if (!a1 && a0.nodeType === 1) {
								a.cleanData(a0.getElementsByTagName("*"));
								a.cleanData([a0]);
							}
							if (a0.parentNode) {
								a0.parentNode.removeChild(a0);
							}
						}
					}
					return this;
				},
				empty : function () {
					for (var aY = 0, aZ; (aZ = this[aY]) != null; aY++) {
						if (aZ.nodeType === 1) {
							a.cleanData(aZ.getElementsByTagName("*"));
						}
						while (aZ.firstChild) {
							aZ.removeChild(aZ.firstChild);
						}
					}
					return this;
				},
				clone : function (aZ) {
					var aY = this.map(function () {
							if (!a.support.noCloneEvent && !a.isXMLDoc(this)) {
								var a1 = this.outerHTML,
								a0 = this.ownerDocument;
								if (!a1) {
									var a2 = a0.createElement("div");
									a2.appendChild(this.cloneNode(true));
									a1 = a2.innerHTML;
								}
								return a.clean([a1.replace(T, "").replace(/=([^="'>\s]+\/)>/g, '="$1">').replace(Z, "")], a0)[0];
							} else {
								return this.cloneNode(true);
							}
						});
					if (aZ === true) {
						q(this, aY);
						q(this.find("*"), aY.find("*"));
					}
					return aY;
				},
				html : function (a0) {
					if (a0 === C) {
						return this[0] && this[0].nodeType === 1 ? this[0].innerHTML.replace(T, "") : null;
					} else {
						if (typeof a0 === "string" && !E.test(a0) && (a.support.leadingWhitespace || !Z.test(a0)) && !ac[(c.exec(a0) || ["", ""])[1].toLowerCase()]) {
							a0 = a0.replace(H, p);
							try {
								for (var aZ = 0, aY = this.length; aZ < aY; aZ++) {
									if (this[aZ].nodeType === 1) {
										a.cleanData(this[aZ].getElementsByTagName("*"));
										this[aZ].innerHTML = a0;
									}
								}
							} catch (a1) {
								this.empty().append(a0);
							}
						} else {
							if (a.isFunction(a0)) {
								this.each(function (a4) {
										var a3 = a(this),
										a2 = a3.html();
										a3.empty().append(function () {
												return a0.call(this, a4, a2);
											});
									});
							} else {
								this.empty().append(a0);
							}
						}
					}
					return this;
				},
				replaceWith : function (aY) {
					if (this[0] && this[0].parentNode) {
						if (a.isFunction(aY)) {
							return this.each(function (a1) {
									var a0 = a(this),
									aZ = a0.html();
									a0.replaceWith(aY.call(this, a1, aZ));
								});
						}
						if (typeof aY !== "string") {
							aY = a(aY).detach();
						}
						return this.each(function () {
								var a0 = this.nextSibling,
								aZ = this.parentNode;
								a(this).remove();
								if (a0) {
									a(a0).before(aY);
								} else {
									a(aZ).append(aY);
								}
							});
					} else {
						return this.pushStack(a(a.isFunction(aY) ? aY() : aY), "replaceWith", aY);
					}
				},
				detach : function (aY) {
					return this.remove(aY, true);
				},
				domManip : function (a4, a9, a8) {
					var a1,
					a2,
					a7 = a4[0],
					aZ = [],
					a3,
					a6;
					if (!a.support.checkClone && arguments.length === 3 && typeof a7 === "string" && l.test(a7)) {
						return this.each(function () {
								a(this).domManip(a4, a9, a8, true);
							});
					}
					if (a.isFunction(a7)) {
						return this.each(function (bb) {
								var ba = a(this);
								a4[0] = a7.call(this, bb, a9 ? ba.html() : C);
								ba.domManip(a4, a9, a8);
							});
					}
					if (this[0]) {
						a6 = a7 && a7.parentNode;
						if (a.support.parentNode && a6 && a6.nodeType === 11 && a6.childNodes.length === this.length) {
							a1 = {
								fragment : a6
							};
						} else {
							a1 = J(a4, this, aZ);
						}
						a3 = a1.fragment;
						if (a3.childNodes.length === 1) {
							a2 = a3 = a3.firstChild;
						} else {
							a2 = a3.firstChild;
						}
						if (a2) {
							a9 = a9 && a.nodeName(a2, "tr");
							for (var a0 = 0, aY = this.length; a0 < aY; a0++) {
								a8.call(a9 ? a5(this[a0], a2) : this[a0], a0 > 0 || a1.cacheable || this.length > 1 ? a3.cloneNode(true) : a3);
							}
						}
						if (aZ.length) {
							a.each(aZ, aV);
						}
					}
					return this;
					function a5(ba, bb) {
						return a.nodeName(ba, "table") ? (ba.getElementsByTagName("tbody")[0] || ba.appendChild(ba.ownerDocument.createElement("tbody"))) : ba;
					}
				}
			});
		function q(a0, aY) {
			var aZ = 0;
			aY.each(function () {
					if (this.nodeName !== (a0[aZ] && a0[aZ].nodeName)) {
						return;
					}
					var a5 = a.data(a0[aZ++]),
					a4 = a.data(this, a5),
					a1 = a5 && a5.events;
					if (a1) {
						delete a4.handle;
						a4.events = {
						};
						for (var a3 in a1) {
							for (var a2 in a1[a3]) {
								a.event.add(this, a3, a1[a3][a2], a1[a3][a2].data);
							}
						}
					}
				});
		}
		function J(a3, a1, aZ) {
			var a2,
			aY,
			a0,
			a4 = (a1 && a1[0] ? a1[0].ownerDocument || a1[0] : ab);
			if (a3.length === 1 && typeof a3[0] === "string" && a3[0].length < 512 && a4 === ab && !E.test(a3[0]) && (a.support.checkClone || !l.test(a3[0]))) {
				aY = true;
				a0 = a.fragments[a3[0]];
				if (a0) {
					if (a0 !== 1) {
						a2 = a0;
					}
				}
			}
			if (!a2) {
				a2 = a4.createDocumentFragment();
				a.clean(a3, a4, a2, aZ);
			}
			if (aY) {
				a.fragments[a3[0]] = a0 ? a2 : 1;
			}
			return{
				fragment : a2,
				cacheable : aY
			};
		}
		a.fragments = {
		};
		a.each({
				appendTo : "append",
				prependTo : "prepend",
				insertBefore : "before",
				insertAfter : "after",
				replaceAll : "replaceWith"
			}, function (aY, aZ) {
				a.fn[aY] = function (a0) {
					var a3 = [],
					a6 = a(a0),
					a5 = this.length === 1 && this[0].parentNode;
					if (a5 && a5.nodeType === 11 && a5.childNodes.length === 1 && a6.length === 1) {
						a6[aZ](this[0]);
						return this;
					} else {
						for (var a4 = 0, a1 = a6.length; a4 < a1; a4++) {
							var a2 = (a4 > 0 ? this.clone(true) : this).get();
							a.fn[aZ].apply(a(a6[a4]), a2);
							a3 = a3.concat(a2);
						}
						return this.pushStack(a3, aY, a6.selector);
					}
				};
			});
		a.extend({
				clean : function (a0, a2, a9, a4) {
					a2 = a2 || ab;
					if (typeof a2.createElement === "undefined") {
						a2 = a2.ownerDocument || a2[0] && a2[0].ownerDocument || ab;
					}
					var ba = [];
					for (var a8 = 0, a3; (a3 = a0[a8]) != null; a8++) {
						if (typeof a3 === "number") {
							a3 += "";
						}
						if (!a3) {
							continue;
						}
						if (typeof a3 === "string" && !L.test(a3)) {
							a3 = a2.createTextNode(a3);
						} else {
							if (typeof a3 === "string") {
								a3 = a3.replace(H, p);
								var bb = (c.exec(a3) || ["", ""])[1].toLowerCase(),
								a1 = ac[bb] || ac._default,
								a7 = a1[0],
								aZ = a2.createElement("div");
								aZ.innerHTML = a1[1] + a3 + a1[2];
								while (a7--) {
									aZ = aZ.lastChild;
								}
								if (!a.support.tbody) {
									var aY = t.test(a3),
									a6 = bb === "table" && !aY ? aZ.firstChild && aZ.firstChild.childNodes : a1[1] === "<table>" && !aY ? aZ.childNodes : [];
									for (var a5 = a6.length - 1; a5 >= 0; --a5) {
										if (a.nodeName(a6[a5], "tbody") && !a6[a5].childNodes.length) {
											a6[a5].parentNode.removeChild(a6[a5]);
										}
									}
								}
								if (!a.support.leadingWhitespace && Z.test(a3)) {
									aZ.insertBefore(a2.createTextNode(Z.exec(a3)[0]), aZ.firstChild);
								}
								a3 = aZ.childNodes;
							}
						}
						if (a3.nodeType) {
							ba.push(a3);
						} else {
							ba = a.merge(ba, a3);
						}
					}
					if (a9) {
						for (var a8 = 0; ba[a8]; a8++) {
							if (a4 && a.nodeName(ba[a8], "script") && (!ba[a8].type || ba[a8].type.toLowerCase() === "text/javascript")) {
								a4.push(ba[a8].parentNode ? ba[a8].parentNode.removeChild(ba[a8]) : ba[a8]);
							} else {
								if (ba[a8].nodeType === 1) {
									ba.splice.apply(ba, [a8 + 1, 0].concat(a.makeArray(ba[a8].getElementsByTagName("script"))));
								}
								a9.appendChild(ba[a8]);
							}
						}
					}
					return ba;
				},
				cleanData : function (aZ) {
					var a2,
					a0,
					aY = a.cache,
					a5 = a.event.special,
					a4 = a.support.deleteExpando;
					for (var a3 = 0, a1; (a1 = aZ[a3]) != null; a3++) {
						a0 = a1[a.expando];
						if (a0) {
							a2 = aY[a0];
							if (a2.events) {
								for (var a6 in a2.events) {
									if (a5[a6]) {
										a.event.remove(a1, a6);
									} else {
										ag(a1, a6, a2.handle);
									}
								}
							}
							if (a4) {
								delete a1[a.expando];
							} else {
								if (a1.removeAttribute) {
									a1.removeAttribute(a.expando);
								}
							}
							delete aY[a0];
						}
					}
				}
			});
		var ar = /z-?index|font-?weight|opacity|zoom|line-?height/i,
		U = /alpha\([^)]*\)/,
		aa = /opacity=([^)]*)/,
		ah = /float/i,
		az = /-([a-z])/ig,
		v = /([A-Z])/g,
		aO = /^-?\d+(?:px)?$/i,
		aU = /^-?\d/,
		aK = {
			position : "absolute",
			visibility : "hidden",
			display : "block"
		},
		W = ["Left", "Right"],
		aE = ["Top", "Bottom"],
		ak = ab.defaultView && ab.defaultView.getComputedStyle,
		aN = a.support.cssFloat ? "cssFloat" : "styleFloat",
		k = function (aY, aZ) {
			return aZ.toUpperCase();
		};
		a.fn.css = function (aY, aZ) {
			return an(this, aY, aZ, true, function (a1, a0, a2) {
					if (a2 === C) {
						return a.curCSS(a1, a0);
					}
					if (typeof a2 === "number" && !ar.test(a0)) {
						a2 += "px";
					}
					a.style(a1, a0, a2);
				});
		};
		a.extend({
				style : function (a2, aZ, a3) {
					if (!a2 || a2.nodeType === 3 || a2.nodeType === 8) {
						return C;
					}
					if ((aZ === "width" || aZ === "height") && parseFloat(a3) < 0) {
						a3 = C;
					}
					var a1 = a2.style || a2,
					a4 = a3 !== C;
					if (!a.support.opacity && aZ === "opacity") {
						if (a4) {
							a1.zoom = 1;
							var aY = parseInt(a3, 10) + "" === "NaN" ? "" : "alpha(opacity=" + a3 * 100 + ")";
							var a0 = a1.filter || a.curCSS(a2, "filter") || "";
							a1.filter = U.test(a0) ? a0.replace(U, aY) : aY;
						}
						return a1.filter && a1.filter.indexOf("opacity=") >= 0 ? (parseFloat(aa.exec(a1.filter)[1]) / 100) + "" : "";
					}
					if (ah.test(aZ)) {
						aZ = aN;
					}
					aZ = aZ.replace(az, k);
					if (a4) {
						a1[aZ] = a3;
					}
					return a1[aZ];
				},
				css : function (a1, aZ, a3, aY) {
					if (aZ === "width" || aZ === "height") {
						var a5,
						a0 = aK,
						a4 = aZ === "width" ? W : aE;
						function a2() {
							a5 = aZ === "width" ? a1.offsetWidth : a1.offsetHeight;
							if (aY === "border") {
								return;
							}
							a.each(a4, function () {
									if (!aY) {
										a5 -= parseFloat(a.curCSS(a1, "padding" + this, true)) || 0;
									}
									if (aY === "margin") {
										a5 += parseFloat(a.curCSS(a1, "margin" + this, true)) || 0;
									} else {
										a5 -= parseFloat(a.curCSS(a1, "border" + this + "Width", true)) || 0;
									}
								});
						}
						if (a1.offsetWidth !== 0) {
							a2();
						} else {
							a.swap(a1, a0, a2);
						}
						return Math.max(0, Math.round(a5));
					}
					return a.curCSS(a1, aZ, a3);
				},
				curCSS : function (a4, aZ, a0) {
					var a7,
					aY = a4.style,
					a1;
					if (!a.support.opacity && aZ === "opacity" && a4.currentStyle) {
						a7 = aa.test(a4.currentStyle.filter || "") ? (parseFloat(RegExp.$1) / 100) + "" : "";
						return a7 === "" ? "1" : a7;
					}
					if (ah.test(aZ)) {
						aZ = aN;
					}
					if (!a0 && aY && aY[aZ]) {
						a7 = aY[aZ];
					} else {
						if (ak) {
							if (ah.test(aZ)) {
								aZ = "float";
							}
							aZ = aZ.replace(v, "-$1").toLowerCase();
							var a6 = a4.ownerDocument.defaultView;
							if (!a6) {
								return null;
							}
							var a8 = a6.getComputedStyle(a4, null);
							if (a8) {
								a7 = a8.getPropertyValue(aZ);
							}
							if (aZ === "opacity" && a7 === "") {
								a7 = "1";
							}
						} else {
							if (a4.currentStyle) {
								var a3 = aZ.replace(az, k);
								a7 = a4.currentStyle[aZ] || a4.currentStyle[a3];
								if (!aO.test(a7) && aU.test(a7)) {
									var a2 = aY.left,
									a5 = a4.runtimeStyle.left;
									a4.runtimeStyle.left = a4.currentStyle.left;
									aY.left = a3 === "fontSize" ? "1em" : (a7 || 0);
									a7 = aY.pixelLeft + "px";
									aY.left = a2;
									a4.runtimeStyle.left = a5;
								}
							}
						}
					}
					return a7;
				},
				swap : function (a1, a0, a2) {
					var aY = {
					};
					for (var aZ in a0) {
						aY[aZ] = a1.style[aZ];
						a1.style[aZ] = a0[aZ];
					}
					a2.call(a1);
					for (var aZ in a0) {
						a1.style[aZ] = aY[aZ];
					}
				}
			});
		if (a.expr && a.expr.filters) {
			a.expr.filters.hidden = function (a1) {
				var aZ = a1.offsetWidth,
				aY = a1.offsetHeight,
				a0 = a1.nodeName.toLowerCase() === "tr";
				return aZ === 0 && aY === 0 && !a0 ? true : aZ > 0 && aY > 0 && !a0 ? false : a.curCSS(a1, "display") === "none";
			};
			a.expr.filters.visible = function (aY) {
				return!a.expr.filters.hidden(aY);
			};
		}
		var af = aP(),
		aJ = /<script(.|\s)*?\/script>/gi,
		o = /select|textarea/i,
		aB = /color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week/i,
		r = /=\?(&|$)/,
		D = /\?/,
		aX = /(\?|&)_=.*?(&|$)/,
		B = /^(\w+:)?\/\/([^\/?#]+)/,
		h = /%20/g,
		w = a.fn.load;
		a.fn.extend({
				load : function (a0, a3, a4) {
					if (typeof a0 !== "string") {
						return w.call(this, a0);
					} else {
						if (!this.length) {
							return this;
						}
					}
					var a2 = a0.indexOf(" ");
					if (a2 >= 0) {
						var aY = a0.slice(a2, a0.length);
						a0 = a0.slice(0, a2);
					}
					var a1 = "GET";
					if (a3) {
						if (a.isFunction(a3)) {
							a4 = a3;
							a3 = null;
						} else {
							if (typeof a3 === "object") {
								a3 = a.param(a3, a.ajaxSettings.traditional);
								a1 = "POST";
							}
						}
					}
					var aZ = this;
					a.ajax({
							url : a0,
							type : a1,
							dataType : "html",
							data : a3,
							complete : function (a6, a5) {
								if (a5 === "success" || a5 === "notmodified") {
									aZ.html(aY ? a("<div />").append(a6.responseText.replace(aJ, "")).find(aY) : a6.responseText);
								}
								if (a4) {
									aZ.each(a4, [a6.responseText, a5, a6]);
								}
							}
						});
					return this;
				},
				serialize : function () {
					return a.param(this.serializeArray());
				},
				serializeArray : function () {
					return this.map(function () {
							return this.elements ? a.makeArray(this.elements) : this;
						}).filter(function () {
							return this.name && !this.disabled && (this.checked || o.test(this.nodeName) || aB.test(this.type));
						}).map(function (aY, aZ) {
							var a0 = a(this).val();
							return a0 == null ? null : a.isArray(a0) ? a.map(a0, function (a2, a1) {
									return{
										name : aZ.name,
										value : a2
									};
								}) : {
								name : aZ.name,
								value : a0
							};
						}).get();
				}
			});
		a.each("ajaxStart ajaxStop ajaxComplete ajaxError ajaxSuccess ajaxSend".split(" "), function (aY, aZ) {
				a.fn[aZ] = function (a0) {
					return this.bind(aZ, a0);
				};
			});
		a.extend({
				get : function (aY, a0, a1, aZ) {
					if (a.isFunction(a0)) {
						aZ = aZ || a1;
						a1 = a0;
						a0 = null;
					}
					return a.ajax({
							type : "GET",
							url : aY,
							data : a0,
							success : a1,
							dataType : aZ
						});
				},
				getScript : function (aY, aZ) {
					return a.get(aY, null, aZ, "script");
				},
				getJSON : function (aY, aZ, a0) {
					return a.get(aY, aZ, a0, "json");
				},
				post : function (aY, a0, a1, aZ) {
					if (a.isFunction(a0)) {
						aZ = aZ || a1;
						a1 = a0;
						a0 = {
						};
					}
					return a.ajax({
							type : "POST",
							url : aY,
							data : a0,
							success : a1,
							dataType : aZ
						});
				},
				ajaxSetup : function (aY) {
					a.extend(a.ajaxSettings, aY);
				},
				ajaxSettings : {
					url : location.href,
					global : true,
					type : "GET",
					contentType : "application/x-www-form-urlencoded",
					processData : true,
					async : true,
					xhr : aM.XMLHttpRequest && (aM.location.protocol !== "file:" || !aM.ActiveXObject) ? function () {
						return new aM.XMLHttpRequest();
					}
					 : function () {
						try {
							return new aM.ActiveXObject("Microsoft.XMLHTTP");
						} catch (aY) {
						}
					},
					accepts : {
						xml : "application/xml, text/xml",
						html : "text/html",
						script : "text/javascript, application/javascript",
						json : "application/json, text/javascript",
						text : "text/plain",
						_default : "*/*"
					}
				},
				lastModified : {
				},
				etag : {
				},
				ajax : function (bd) {
					var a8 = a.extend(true, {
						}, a.ajaxSettings, bd);
					var bi,
					bc,
					bh,
					bj = bd && bd.context || a8,
					a0 = a8.type.toUpperCase();
					if (a8.data && a8.processData && typeof a8.data !== "string") {
						a8.data = a.param(a8.data, a8.traditional);
					}
					if (a8.dataType === "jsonp") {
						if (a0 === "GET") {
							if (!r.test(a8.url)) {
								a8.url += (D.test(a8.url) ? "&" : "?") + (a8.jsonp || "callback") + "=?";
							}
						} else {
							if (!a8.data || !r.test(a8.data)) {
								a8.data = (a8.data ? a8.data + "&" : "") + (a8.jsonp || "callback") + "=?";
							}
						}
						a8.dataType = "json";
					}
					if (a8.dataType === "json" && (a8.data && r.test(a8.data) || r.test(a8.url))) {
						bi = a8.jsonpCallback || ("jsonp" + af++);
						if (a8.data) {
							a8.data = (a8.data + "").replace(r, "=" + bi + "$1");
						}
						a8.url = a8.url.replace(r, "=" + bi + "$1");
						a8.dataType = "script";
						aM[bi] = aM[bi] || function (bk) {
							bh = bk;
							a3();
							a6();
							aM[bi] = C;
							try {
								delete aM[bi];
							} catch (bl) {
							}
							if (a1) {
								a1.removeChild(bf);
							}
						};
					}
					if (a8.dataType === "script" && a8.cache === null) {
						a8.cache = false;
					}
					if (a8.cache === false && a0 === "GET") {
						var aY = aP();
						var bg = a8.url.replace(aX, "$1_=" + aY + "$2");
						a8.url = bg + ((bg === a8.url) ? (D.test(a8.url) ? "&" : "?") + "_=" + aY : "");
					}
					if (a8.data && a0 === "GET") {
						a8.url += (D.test(a8.url) ? "&" : "?") + a8.data;
					}
					if (a8.global && !a.active++) {
						a.event.trigger("ajaxStart");
					}
					var bb = B.exec(a8.url),
					a2 = bb && (bb[1] && bb[1] !== location.protocol || bb[2] !== location.host);
					if (a8.dataType === "script" && a0 === "GET" && a2) {
						var a1 = ab.getElementsByTagName("head")[0] || ab.documentElement;
						var bf = ab.createElement("script");
						bf.src = a8.url;
						if (a8.scriptCharset) {
							bf.charset = a8.scriptCharset;
						}
						if (!bi) {
							var ba = false;
							bf.onload = bf.onreadystatechange = function () {
								if (!ba && (!this.readyState || this.readyState === "loaded" || this.readyState === "complete")) {
									ba = true;
									a3();
									a6();
									bf.onload = bf.onreadystatechange = null;
									if (a1 && bf.parentNode) {
										a1.removeChild(bf);
									}
								}
							};
						}
						a1.insertBefore(bf, a1.firstChild);
						return C;
					}
					var a5 = false;
					var a4 = a8.xhr();
					if (!a4) {
						return;
					}
					if (a8.username) {
						a4.open(a0, a8.url, a8.async, a8.username, a8.password);
					} else {
						a4.open(a0, a8.url, a8.async);
					}
					try {
						if (a8.data || bd && bd.contentType) {
							a4.setRequestHeader("Content-Type", a8.contentType);
						}
						if (a8.ifModified) {
							if (a.lastModified[a8.url]) {
								a4.setRequestHeader("If-Modified-Since", a.lastModified[a8.url]);
							}
							if (a.etag[a8.url]) {
								a4.setRequestHeader("If-None-Match", a.etag[a8.url]);
							}
						}
						if (!a2) {
							a4.setRequestHeader("X-Requested-With", "XMLHttpRequest");
						}
						a4.setRequestHeader("Accept", a8.dataType && a8.accepts[a8.dataType] ? a8.accepts[a8.dataType] + ", */*" : a8.accepts._default);
					} catch (be) {
					}
					if (a8.beforeSend && a8.beforeSend.call(bj, a4, a8) === false) {
						if (a8.global && !--a.active) {
							a.event.trigger("ajaxStop");
						}
						a4.abort();
						return false;
					}
					if (a8.global) {
						a9("ajaxSend", [a4, a8]);
					}
					var a7 = a4.onreadystatechange = function (bk) {
						if (!a4 || a4.readyState === 0 || bk === "abort") {
							if (!a5) {
								a6();
							}
							a5 = true;
							if (a4) {
								a4.onreadystatechange = a.noop;
							}
						} else {
							if (!a5 && a4 && (a4.readyState === 4 || bk === "timeout")) {
								a5 = true;
								a4.onreadystatechange = a.noop;
								bc = bk === "timeout" ? "timeout" : !a.httpSuccess(a4) ? "error" : a8.ifModified && a.httpNotModified(a4, a8.url) ? "notmodified" : "success";
								var bm;
								if (bc === "success") {
									try {
										bh = a.httpData(a4, a8.dataType, a8);
									} catch (bl) {
										bc = "parsererror";
										bm = bl;
									}
								}
								if (bc === "success" || bc === "notmodified") {
									if (!bi) {
										a3();
									}
								} else {
									a.handleError(a8, a4, bc, bm);
								}
								a6();
								if (bk === "timeout") {
									a4.abort();
								}
								if (a8.async) {
									a4 = null;
								}
							}
						}
					};
					try {
						var aZ = a4.abort;
						a4.abort = function () {
							if (a4) {
								aZ.call(a4);
							}
							a7("abort");
						};
					} catch (be) {
					}
					if (a8.async && a8.timeout > 0) {
						setTimeout(function () {
								if (a4 && !a5) {
									a7("timeout");
								}
							}, a8.timeout);
					}
					try {
						a4.send(a0 === "POST" || a0 === "PUT" || a0 === "DELETE" ? a8.data : null);
					} catch (be) {
						a.handleError(a8, a4, null, be);
						a6();
					}
					if (!a8.async) {
						a7();
					}
					function a3() {
						if (a8.success) {
							a8.success.call(bj, bh, bc, a4);
						}
						if (a8.global) {
							a9("ajaxSuccess", [a4, a8]);
						}
					}
					function a6() {
						if (a8.complete) {
							a8.complete.call(bj, a4, bc);
						}
						if (a8.global) {
							a9("ajaxComplete", [a4, a8]);
						}
						if (a8.global && !--a.active) {
							a.event.trigger("ajaxStop");
						}
					}
					function a9(bl, bk) {
						(a8.context ? a(a8.context) : a.event).trigger(bl, bk);
					}
					return a4;
				},
				handleError : function (aZ, a1, aY, a0) {
					if (aZ.error) {
						aZ.error.call(aZ.context || aZ, a1, aY, a0);
					}
					if (aZ.global) {
						(aZ.context ? a(aZ.context) : a.event).trigger("ajaxError", [a1, aZ, a0]);
					}
				},
				active : 0,
				httpSuccess : function (aZ) {
					try {
						return!aZ.status && location.protocol === "file:" || (aZ.status >= 200 && aZ.status < 300) || aZ.status === 304 || aZ.status === 1223 || aZ.status === 0;
					} catch (aY) {
					}
					return false;
				},
				httpNotModified : function (a1, aY) {
					var a0 = a1.getResponseHeader("Last-Modified"),
					aZ = a1.getResponseHeader("Etag");
					if (a0) {
						a.lastModified[aY] = a0;
					}
					if (aZ) {
						a.etag[aY] = aZ;
					}
					return a1.status === 304 || a1.status === 0;
				},
				httpData : function (a3, a1, a0) {
					var aZ = a3.getResponseHeader("content-type") || "",
					aY = a1 === "xml" || !a1 && aZ.indexOf("xml") >= 0,
					a2 = aY ? a3.responseXML : a3.responseText;
					if (aY && a2.documentElement.nodeName === "parsererror") {
						a.error("parsererror");
					}
					if (a0 && a0.dataFilter) {
						a2 = a0.dataFilter(a2, a1);
					}
					if (typeof a2 === "string") {
						if (a1 === "json" || !a1 && aZ.indexOf("json") >= 0) {
							a2 = a.parseJSON(a2);
						} else {
							if (a1 === "script" || !a1 && aZ.indexOf("javascript") >= 0) {
								a.globalEval(a2);
							}
						}
					}
					return a2;
				},
				param : function (aY, a1) {
					var aZ = [];
					if (a1 === C) {
						a1 = a.ajaxSettings.traditional;
					}
					if (a.isArray(aY) || aY.jquery) {
						a.each(aY, function () {
								a3(this.name, this.value);
							});
					} else {
						for (var a2 in aY) {
							a0(a2, aY[a2]);
						}
					}
					return aZ.join("&").replace(h, "+");
					function a0(a4, a5) {
						if (a.isArray(a5)) {
							a.each(a5, function (a7, a6) {
									if (a1 || /\[\]$/.test(a4)) {
										a3(a4, a6);
									} else {
										a0(a4 + "[" + (typeof a6 === "object" || a.isArray(a6) ? a7 : "") + "]", a6);
									}
								});
						} else {
							if (!a1 && a5 != null && typeof a5 === "object") {
								a.each(a5, function (a7, a6) {
										a0(a4 + "[" + a7 + "]", a6);
									});
							} else {
								a3(a4, a5);
							}
						}
					}
					function a3(a4, a5) {
						a5 = a.isFunction(a5) ? a5() : a5;
						aZ[aZ.length] = encodeURIComponent(a4) + "=" + encodeURIComponent(a5);
					}
				}
			});
		var G = {
		},
		ae = /toggle|show|hide/,
		au = /^([+-]=)?([\d+-.]+)(.*)$/,
		aF,
		aj = [["height", "marginTop", "marginBottom", "paddingTop", "paddingBottom"], ["width", "marginLeft", "marginRight", "paddingLeft", "paddingRight"], ["opacity"]];
		a.fn.extend({
				show : function (aZ, a7) {
					if (aZ || aZ === 0) {
						return this.animate(aD("show", 3), aZ, a7);
					} else {
						for (var a4 = 0, a1 = this.length; a4 < a1; a4++) {
							var aY = a.data(this[a4], "olddisplay");
							this[a4].style.display = aY || "";
							if (a.css(this[a4], "display") === "none") {
								var a6 = this[a4].nodeName,
								a5;
								if (G[a6]) {
									a5 = G[a6];
								} else {
									var a0 = a("<" + a6 + " />").appendTo("body");
									a5 = a0.css("display");
									if (a5 === "none") {
										a5 = "block";
									}
									a0.remove();
									G[a6] = a5;
								}
								a.data(this[a4], "olddisplay", a5);
							}
						}
						for (var a3 = 0, a2 = this.length; a3 < a2; a3++) {
							this[a3].style.display = a.data(this[a3], "olddisplay") || "";
						}
						return this;
					}
				},
				hide : function (a3, a4) {
					if (a3 || a3 === 0) {
						return this.animate(aD("hide", 3), a3, a4);
					} else {
						for (var a2 = 0, aZ = this.length; a2 < aZ; a2++) {
							var aY = a.data(this[a2], "olddisplay");
							if (!aY && aY !== "none") {
								a.data(this[a2], "olddisplay", a.css(this[a2], "display"));
							}
						}
						for (var a1 = 0, a0 = this.length; a1 < a0; a1++) {
							this[a1].style.display = "none";
						}
						return this;
					}
				},
				_toggle : a.fn.toggle,
				toggle : function (a0, aZ) {
					var aY = typeof a0 === "boolean";
					if (a.isFunction(a0) && a.isFunction(aZ)) {
						this._toggle.apply(this, arguments);
					} else {
						if (a0 == null || aY) {
							this.each(function () {
									var a1 = aY ? a0 : a(this).is(":hidden");
									a(this)[a1 ? "show" : "hide"]();
								});
						} else {
							this.animate(aD("toggle", 3), a0, aZ);
						}
					}
					return this;
				},
				fadeTo : function (aY, a0, aZ) {
					return this.filter(":hidden").css("opacity", 0).show().end().animate({
							opacity : a0
						}, aY, aZ);
				},
				animate : function (a2, aZ, a1, a0) {
					var aY = a.speed(aZ, a1, a0);
					if (a.isEmptyObject(a2)) {
						return this.each(aY.complete);
					}
					return this[aY.queue === false ? "each" : "queue"](function () {
							var a5 = a.extend({
								}, aY),
							a7,
							a6 = this.nodeType === 1 && a(this).is(":hidden"),
							a3 = this;
							for (a7 in a2) {
								var a4 = a7.replace(az, k);
								if (a7 !== a4) {
									a2[a4] = a2[a7];
									delete a2[a7];
									a7 = a4;
								}
								if (a2[a7] === "hide" && a6 || a2[a7] === "show" && !a6) {
									return a5.complete.call(this);
								}
								if ((a7 === "height" || a7 === "width") && this.style) {
									a5.display = a.css(this, "display");
									a5.overflow = this.style.overflow;
								}
								if (a.isArray(a2[a7])) {
									(a5.specialEasing = a5.specialEasing || {
										})[a7] = a2[a7][1];
									a2[a7] = a2[a7][0];
								}
							}
							if (a5.overflow != null) {
								this.style.overflow = "hidden";
							}
							a5.curAnim = a.extend({
								}, a2);
							a.each(a2, function (a9, bd) {
									var bc = new a.fx(a3, a5, a9);
									if (ae.test(bd)) {
										bc[bd === "toggle" ? a6 ? "show" : "hide" : bd](a2);
									} else {
										var bb = au.exec(bd),
										be = bc.cur(true) || 0;
										if (bb) {
											var a8 = parseFloat(bb[2]),
											ba = bb[3] || "px";
											if (ba !== "px") {
												a3.style[a9] = (a8 || 1) + ba;
												be = ((a8 || 1) / bc.cur(true)) * be;
												a3.style[a9] = be + ba;
											}
											if (bb[1]) {
												a8 = ((bb[1] === "-=" ? -1 : 1) * a8) + be;
											}
											bc.custom(be, a8, ba);
										} else {
											bc.custom(be, bd, "");
										}
									}
								});
							return true;
						});
				},
				stop : function (aZ, aY) {
					var a0 = a.timers;
					if (aZ) {
						this.queue([]);
					}
					this.each(function () {
							for (var a1 = a0.length - 1; a1 >= 0; a1--) {
								if (a0[a1].elem === this) {
									if (aY) {
										a0[a1](true);
									}
									a0.splice(a1, 1);
								}
							}
						});
					if (!aY) {
						this.dequeue();
					}
					return this;
				}
			});
		a.each({
				slideDown : aD("show", 1),
				slideUp : aD("hide", 1),
				slideToggle : aD("toggle", 1),
				fadeIn : {
					opacity : "show"
				},
				fadeOut : {
					opacity : "hide"
				}
			}, function (aY, aZ) {
				a.fn[aY] = function (a0, a1) {
					return this.animate(aZ, a0, a1);
				};
			});
		a.extend({
				speed : function (a0, a1, aZ) {
					var aY = a0 && typeof a0 === "object" ? a0 : {
						complete : aZ || !aZ && a1 || a.isFunction(a0) && a0,
						duration : a0,
						easing : aZ && a1 || a1 && !a.isFunction(a1) && a1
					};
					aY.duration = a.fx.off ? 0 : typeof aY.duration === "number" ? aY.duration : a.fx.speeds[aY.duration] || a.fx.speeds._default;
					aY.old = aY.complete;
					aY.complete = function () {
						if (aY.queue !== false) {
							a(this).dequeue();
						}
						if (a.isFunction(aY.old)) {
							aY.old.call(this);
						}
					};
					return aY;
				},
				easing : {
					linear : function (a0, a1, aY, aZ) {
						return aY + aZ * a0;
					},
					swing : function (a0, a1, aY, aZ) {
						return((-Math.cos(a0 * Math.PI) / 2) + 0.5) * aZ + aY;
					}
				},
				timers : [],
				fx : function (aZ, aY, a0) {
					this.options = aY;
					this.elem = aZ;
					this.prop = a0;
					if (!aY.orig) {
						aY.orig = {
						};
					}
				}
			});
		a.fx.prototype = {
			update : function () {
				if (this.options.step) {
					this.options.step.call(this.elem, this.now, this);
				}
				(a.fx.step[this.prop] || a.fx.step._default)(this);
				if ((this.prop === "height" || this.prop === "width") && this.elem.style) {
					this.elem.style.display = "block";
				}
			},
			cur : function (aZ) {
				if (this.elem[this.prop] != null && (!this.elem.style || this.elem.style[this.prop] == null)) {
					return this.elem[this.prop];
				}
				var aY = parseFloat(a.css(this.elem, this.prop, aZ));
				return aY && aY > -10000 ? aY : parseFloat(a.curCSS(this.elem, this.prop)) || 0;
			},
			custom : function (a2, a1, a0) {
				this.startTime = aP();
				this.start = a2;
				this.end = a1;
				this.unit = a0 || this.unit || "px";
				this.now = this.start;
				this.pos = this.state = 0;
				var aY = this;
				function aZ(a3) {
					return aY.step(a3);
				}
				aZ.elem = this.elem;
				if (aZ() && a.timers.push(aZ) && !aF) {
					aF = setInterval(a.fx.tick, 13);
				}
			},
			show : function () {
				this.options.orig[this.prop] = a.style(this.elem, this.prop);
				this.options.show = true;
				this.custom(this.prop === "width" || this.prop === "height" ? 1 : 0, this.cur());
				a(this.elem).show();
			},
			hide : function () {
				this.options.orig[this.prop] = a.style(this.elem, this.prop);
				this.options.hide = true;
				this.custom(this.cur(), 0);
			},
			step : function (a1) {
				var a6 = aP(),
				a2 = true;
				if (a1 || a6 >= this.options.duration + this.startTime) {
					this.now = this.end;
					this.pos = this.state = 1;
					this.update();
					this.options.curAnim[this.prop] = true;
					for (var a3 in this.options.curAnim) {
						if (this.options.curAnim[a3] !== true) {
							a2 = false;
						}
					}
					if (a2) {
						if (this.options.display != null) {
							this.elem.style.overflow = this.options.overflow;
							var a0 = a.data(this.elem, "olddisplay");
							this.elem.style.display = a0 ? a0 : this.options.display;
							if (a.css(this.elem, "display") === "none") {
								this.elem.style.display = "block";
							}
						}
						if (this.options.hide) {
							a(this.elem).hide();
						}
						if (this.options.hide || this.options.show) {
							for (var aY in this.options.curAnim) {
								a.style(this.elem, aY, this.options.orig[aY]);
							}
						}
						this.options.complete.call(this.elem);
					}
					return false;
				} else {
					var aZ = a6 - this.startTime;
					this.state = aZ / this.options.duration;
					var a4 = this.options.specialEasing && this.options.specialEasing[this.prop];
					var a5 = this.options.easing || (a.easing.swing ? "swing" : "linear");
					this.pos = a.easing[a4 || a5](this.state, aZ, 0, 1, this.options.duration);
					this.now = this.start + ((this.end - this.start) * this.pos);
					this.update();
				}
				return true;
			}
		};
		a.extend(a.fx, {
				tick : function () {
					var aZ = a.timers;
					for (var aY = 0; aY < aZ.length; aY++) {
						if (!aZ[aY]()) {
							aZ.splice(aY--, 1);
						}
					}
					if (!aZ.length) {
						a.fx.stop();
					}
				},
				stop : function () {
					clearInterval(aF);
					aF = null;
				},
				speeds : {
					slow : 600,
					fast : 200,
					_default : 400
				},
				step : {
					opacity : function (aY) {
						a.style(aY.elem, "opacity", aY.now);
					},
					_default : function (aY) {
						if (aY.elem.style && aY.elem.style[aY.prop] != null) {
							aY.elem.style[aY.prop] = (aY.prop === "width" || aY.prop === "height" ? Math.max(0, aY.now) : aY.now) + aY.unit;
						} else {
							aY.elem[aY.prop] = aY.now;
						}
					}
				}
			});
		if (a.expr && a.expr.filters) {
			a.expr.filters.animated = function (aY) {
				return a.grep(a.timers, function (aZ) {
						return aY === aZ.elem;
					}).length;
			};
		}
		function aD(aZ, aY) {
			var a0 = {
			};
			a.each(aj.concat.apply([], aj.slice(0, aY)), function () {
					a0[this] = aZ;
				});
			return a0;
		}
		if ("getBoundingClientRect" in ab.documentElement) {
			a.fn.offset = function (a7) {
				var a0 = this[0];
				if (a7) {
					return this.each(function (a8) {
							a.offset.setOffset(this, a7, a8);
						});
				}
				if (!a0 || !a0.ownerDocument) {
					return null;
				}
				if (a0 === a0.ownerDocument.body) {
					return a.offset.bodyOffset(a0);
				}
				var a2 = a0.getBoundingClientRect(),
				a6 = a0.ownerDocument,
				a3 = a6.body,
				aY = a6.documentElement,
				a1 = aY.clientTop || a3.clientTop || 0,
				a4 = aY.clientLeft || a3.clientLeft || 0,
				a5 = a2.top + (self.pageYOffset || a.support.boxModel && aY.scrollTop || a3.scrollTop) - a1,
				aZ = a2.left + (self.pageXOffset || a.support.boxModel && aY.scrollLeft || a3.scrollLeft) - a4;
				return{
					top : a5,
					left : aZ
				};
			};
		} else {
			a.fn.offset = function (a9) {
				var a3 = this[0];
				if (a9) {
					return this.each(function (ba) {
							a.offset.setOffset(this, a9, ba);
						});
				}
				if (!a3 || !a3.ownerDocument) {
					return null;
				}
				if (a3 === a3.ownerDocument.body) {
					return a.offset.bodyOffset(a3);
				}
				a.offset.initialize();
				var a0 = a3.offsetParent,
				aZ = a3,
				a8 = a3.ownerDocument,
				a6,
				a1 = a8.documentElement,
				a4 = a8.body,
				a5 = a8.defaultView,
				aY = a5 ? a5.getComputedStyle(a3, null) : a3.currentStyle,
				a7 = a3.offsetTop,
				a2 = a3.offsetLeft;
				while ((a3 = a3.parentNode) && a3 !== a4 && a3 !== a1) {
					if (a.offset.supportsFixedPosition && aY.position === "fixed") {
						break;
					}
					a6 = a5 ? a5.getComputedStyle(a3, null) : a3.currentStyle;
					a7 -= a3.scrollTop;
					a2 -= a3.scrollLeft;
					if (a3 === a0) {
						a7 += a3.offsetTop;
						a2 += a3.offsetLeft;
						if (a.offset.doesNotAddBorder && !(a.offset.doesAddBorderForTableAndCells && /^t(able|d|h)$/i.test(a3.nodeName))) {
							a7 += parseFloat(a6.borderTopWidth) || 0;
							a2 += parseFloat(a6.borderLeftWidth) || 0;
						}
						aZ = a0,
						a0 = a3.offsetParent;
					}
					if (a.offset.subtractsBorderForOverflowNotVisible && a6.overflow !== "visible") {
						a7 += parseFloat(a6.borderTopWidth) || 0;
						a2 += parseFloat(a6.borderLeftWidth) || 0;
					}
					aY = a6;
				}
				if (aY.position === "relative" || aY.position === "static") {
					a7 += a4.offsetTop;
					a2 += a4.offsetLeft;
				}
				if (a.offset.supportsFixedPosition && aY.position === "fixed") {
					a7 += Math.max(a1.scrollTop, a4.scrollTop);
					a2 += Math.max(a1.scrollLeft, a4.scrollLeft);
				}
				return{
					top : a7,
					left : a2
				};
			};
		}
		a.offset = {
			initialize : function () {
				var aY = ab.body,
				aZ = ab.createElement("div"),
				a2,
				a4,
				a3,
				a5,
				a0 = parseFloat(a.curCSS(aY, "marginTop", true)) || 0,
				a1 = "<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";
				a.extend(aZ.style, {
						position : "absolute",
						top : 0,
						left : 0,
						margin : 0,
						border : 0,
						width : "1px",
						height : "1px",
						visibility : "hidden"
					});
				aZ.innerHTML = a1;
				aY.insertBefore(aZ, aY.firstChild);
				a2 = aZ.firstChild;
				a4 = a2.firstChild;
				a5 = a2.nextSibling.firstChild.firstChild;
				this.doesNotAddBorder = (a4.offsetTop !== 5);
				this.doesAddBorderForTableAndCells = (a5.offsetTop === 5);
				a4.style.position = "fixed",
				a4.style.top = "20px";
				this.supportsFixedPosition = (a4.offsetTop === 20 || a4.offsetTop === 15);
				a4.style.position = a4.style.top = "";
				a2.style.overflow = "hidden",
				a2.style.position = "relative";
				this.subtractsBorderForOverflowNotVisible = (a4.offsetTop === -5);
				this.doesNotIncludeMarginInBodyOffset = (aY.offsetTop !== a0);
				aY.removeChild(aZ);
				aY = aZ = a2 = a4 = a3 = a5 = null;
				a.offset.initialize = a.noop;
			},
			bodyOffset : function (aY) {
				var a0 = aY.offsetTop,
				aZ = aY.offsetLeft;
				a.offset.initialize();
				if (a.offset.doesNotIncludeMarginInBodyOffset) {
					a0 += parseFloat(a.curCSS(aY, "marginTop", true)) || 0;
					aZ += parseFloat(a.curCSS(aY, "marginLeft", true)) || 0;
				}
				return{
					top : a0,
					left : aZ
				};
			},
			setOffset : function (a3, aZ, a0) {
				if (/static/.test(a.curCSS(a3, "position"))) {
					a3.style.position = "relative";
				}
				var a2 = a(a3),
				a5 = a2.offset(),
				aY = parseInt(a.curCSS(a3, "top", true), 10) || 0,
				a4 = parseInt(a.curCSS(a3, "left", true), 10) || 0;
				if (a.isFunction(aZ)) {
					aZ = aZ.call(a3, a0, a5);
				}
				var a1 = {
					top : (aZ.top - a5.top) + aY,
					left : (aZ.left - a5.left) + a4
				};
				if ("using" in aZ) {
					aZ.using.call(a3, a1);
				} else {
					a2.css(a1);
				}
			}
		};
		a.fn.extend({
				position : function () {
					if (!this[0]) {
						return null;
					}
					var a0 = this[0],
					aZ = this.offsetParent(),
					a1 = this.offset(),
					aY = /^body|html$/i.test(aZ[0].nodeName) ? {
						top : 0,
						left : 0
					}
					 : aZ.offset();
					a1.top -= parseFloat(a.curCSS(a0, "marginTop", true)) || 0;
					a1.left -= parseFloat(a.curCSS(a0, "marginLeft", true)) || 0;
					aY.top += parseFloat(a.curCSS(aZ[0], "borderTopWidth", true)) || 0;
					aY.left += parseFloat(a.curCSS(aZ[0], "borderLeftWidth", true)) || 0;
					return{
						top : a1.top - aY.top,
						left : a1.left - aY.left
					};
				},
				offsetParent : function () {
					return this.map(function () {
							var aY = this.offsetParent || ab.body;
							while (aY && (!/^body|html$/i.test(aY.nodeName) && a.css(aY, "position") === "static")) {
								aY = aY.offsetParent;
							}
							return aY;
						});
				}
			});
		a.each(["Left", "Top"], function (aZ, aY) {
				var a0 = "scroll" + aY;
				a.fn[a0] = function (a3) {
					var a1 = this[0],
					a2;
					if (!a1) {
						return null;
					}
					if (a3 !== C) {
						return this.each(function () {
								a2 = am(this);
								if (a2) {
									a2.scrollTo(!aZ ? a3 : a(a2).scrollLeft(), aZ ? a3 : a(a2).scrollTop());
								} else {
									this[a0] = a3;
								}
							});
					} else {
						a2 = am(a1);
						return a2 ? ("pageXOffset" in a2) ? a2[aZ ? "pageYOffset" : "pageXOffset"] : a.support.boxModel && a2.document.documentElement[a0] || a2.document.body[a0] : a1[a0];
					}
				};
			});
		function am(aY) {
			return("scrollTo" in aY && aY.document) ? aY : aY.nodeType === 9 ? aY.defaultView || aY.parentWindow : false;
		}
		a.each(["Height", "Width"], function (aZ, aY) {
				var a0 = aY.toLowerCase();
				a.fn["inner" + aY] = function () {
					return this[0] ? a.css(this[0], a0, false, "padding") : null;
				};
				a.fn["outer" + aY] = function (a1) {
					return this[0] ? a.css(this[0], a0, false, a1 ? "margin" : "border") : null;
				};
				a.fn[a0] = function (a1) {
					var a2 = this[0];
					if (!a2) {
						return a1 == null ? null : this;
					}
					if (a.isFunction(a1)) {
						return this.each(function (a4) {
								var a3 = a(this);
								a3[a0](a1.call(this, a4, a3[a0]()));
							});
					}
					return("scrollTo" in a2 && a2.document) ? a2.document.compatMode === "CSS1Compat" && a2.document.documentElement["client" + aY] || a2.document.body["client" + aY] : (a2.nodeType === 9) ? Math.max(a2.documentElement["client" + aY], a2.body["scroll" + aY], a2.documentElement["scroll" + aY], a2.body["offset" + aY], a2.documentElement["offset" + aY]) : a1 === C ? a.css(a2, a0) : this.css(a0, typeof a1 === "string" ? a1 : a1 + "px");
				};
			});
		aM.jQuery = aM.$ = a;
	})(window);
jQuery.cookie = function (b, j, m) {
	if (typeof j != "undefined") {
		m = m || {
		};
		if (j === null) {
			j = "";
			m.expires = -1;
		}
		var e = "";
		if (m.expires && (typeof m.expires == "number" || m.expires.toUTCString)) {
			var f;
			if (typeof m.expires == "number") {
				f = new Date((new Date()).getTime() + (m.expires * 24 * 60 * 60 * 1000));
			} else {
				f = m.expires;
			}
			e = "; expires=" + f.toUTCString();
		}
		var l = m.path ? "; path=" + (m.path) : "";
		var g = m.domain ? "; domain=" + (m.domain) : "";
		var a = m.secure ? "; secure" : "";
		document.cookie = [b, "=", encodeURIComponent(j), e, l, g, a].join("");
	} else {
		var d = null;
		if (document.cookie && document.cookie != "") {
			var k = document.cookie.split(";");
			for (var h = 0; h < k.length; h++) {
				var c = jQuery.trim(k[h]);
				if (c.substring(0, b.length + 1) == (b + "=")) {
					d = decodeURIComponent(c.substring(b.length + 1));
					break;
				}
			}
		}
		return d;
	}
};
jQuery.cookieAdvanced = function (z, m, n, b, y) {
	b = $.extend({
		}, {
			expires : 30,
			path : "/",
			domain : "i.maxthon.cn",
			secure : false
		}, b);
	if (y === undefined) {
		y = true;
	}
	if (typeof n != "undefined") {
		b = b || {
		};
		if (n === null) {
			n = "";
			b.expires = -1;
		}
		var v = "";
		if (b.expires && (typeof b.expires == "number" || b.expires.toUTCString)) {
			var u;
			if (typeof b.expires == "number") {
				u = new Date((new Date()).getTime() + (b.expires * 24 * 60 * 60 * 1000));
			} else {
				u = b.expires;
			}
			v = "; expires=" + u.toUTCString();
		}
		var k = b.path ? "; path=" + (b.path) : ";path=/";
		var w = b.domain ? "; domain=" + (b.domain) : "";
		var h = b.secure ? "; secure" : "";
		if (m != null && m != "") {
			if (y) {
				var g = $.cookieAdvanced(z);
				var o = {
				};
				if (g != null) {
					var e = g.split("&");
					$(e).each(function (i, p) {
							var B,
							A,
							j;
							B = p.split("=");
							A = B[0];
							j = B[1];
							o[A] = j;
						});
				}
				o[m] = n;
				var x = [];
				for (var l in o) {
					x.push(l + "=" + encodeURIComponent(o[l]));
				}
				n = x.join("&");
			} else {
				n = m + "=" + encodeURIComponent(n);
			}
		}
		document.cookie = [z, "=", n, v, k, w, h].join("");
	} else {
		var d = null;
		if (document.cookie && document.cookie != "") {
			var a = document.cookie.split(";");
			for (var r = 0; r < a.length; r++) {
				var t = jQuery.trim(a[r]);
				if (t.substring(0, z.length + 1) == (z + "=")) {
					d = decodeURIComponent(t.substring(z.length + 1));
					if (typeof m != "undefined" && m != null && m != "") {
						var f = d.toString().split("&");
						var s = null;
						for (var q = 0; q < f.length; q++) {
							var c = jQuery.trim(f[q]);
							if (c.substring(0, m.length + 1) == (m + "=")) {
								s = decodeURIComponent(c.substring(m.length + 1));
								break;
							}
						}
						d = s;
					}
					break;
				}
			}
		}
		return d;
	}
};
(function () {
		if ($.cookieAdvanced("ch.new") != null) {
			return;
		}
		function a(c) {
			var b = $.cookieAdvanced(c, null);
			if (b == null) {
				return;
			}
			$.cookieAdvanced("ch.new", c, b);
			$.cookie(c, null);
		}
		a("main-tab");
		a("current-tool");
		a("constellation");
		a("widget-center-mode");
		a("defaultPopup");
		a("music-autoclose");
		$.cookie("search-engine", null);
	})();
(function (d) {
		d.tools = d.tools || {
			version : "1.2.3"
		};
		d.tools.tabs = {
			conf : {
				tabs : "a",
				current : "current",
				onBeforeClick : null,
				onClick : null,
				effect : "default",
				initialIndex : 0,
				event : "click",
				rotate : false,
				history : false
			},
			addEffect : function (e, f) {
				c[e] = f;
			}
		};
		var c = {
			"default" : function (f, e) {
				this.getPanes().hide().eq(f).show();
				e.call();
			},
			fade : function (g, e) {
				var f = this.getConf(),
				j = f.fadeOutSpeed,
				h = this.getPanes();
				if (j) {
					h.fadeOut(j);
				} else {
					h.hide();
				}
				h.eq(g).fadeIn(f.fadeInSpeed, e);
			},
			slide : function (f, e) {
				this.getPanes().slideUp(200);
				this.getPanes().eq(f).slideDown(400, e);
			},
			ajax : function (f, e) {
				this.getPanes().eq(0).load(this.getTabs().eq(f).attr("href"), e);
			}
		};
		var b;
		d.tools.tabs.addEffect("horizontal", function (f, e) {
				if (!b) {
					b = this.getPanes().eq(0).width();
				}
				this.getCurrentPane().animate({
						width : 0
					}, function () {
						d(this).hide();
					});
				this.getPanes().eq(f).animate({
						width : b
					}, function () {
						d(this).show();
						e.call();
					});
			});
		d.tools.tabs.addEffect("folderForWidget", function (f, e) {
				e.call();
			});
		function a(e, j, h) {
			var f = this,
			g = e.add(this),
			i = e.find(h.tabs),
			k = j.jquery ? j : e.children(j),
			l;
			if (!i.length) {
				i = e.children();
			}
			if (!k.length) {
				k = e.parent().find(j);
			}
			if (!k.length) {
				k = d(j);
			}
			d.extend(this, {
					click : function (m, p) {
						var n = i.eq(m);
						if (typeof m == "string" && m.replace("#", "")) {
							n = i.filter("[href*=" + m.replace("#", "") + "]");
							m = Math.max(i.index(n), 0);
						}
						if (h.rotate) {
							var o = i.length - 1;
							if (m < 0) {
								return f.click(o, p);
							}
							if (m > o) {
								return f.click(0, p);
							}
						}
						if (!n.length) {
							if (l >= 0) {
								return f;
							}
							m = h.initialIndex;
							n = i.eq(m);
						}
						if (m === l) {
							return f;
						}
						p = p || d.Event();
						p.type = "onBeforeClick";
						g.trigger(p, [m]);
						if (p.isDefaultPrevented()) {
							return;
						}
						c[h.effect].call(f, m, function () {
								p.type = "onClick";
								g.trigger(p, [m]);
							});
						l = m;
						i.removeClass(h.current);
						n.addClass(h.current);
						return f;
					},
					getConf : function () {
						return h;
					},
					getTabs : function () {
						return i;
					},
					getPanes : function () {
						return k;
					},
					getCurrentPane : function () {
						return k.eq(l);
					},
					getCurrentTab : function () {
						return i.eq(l);
					},
					getIndex : function () {
						return l;
					},
					next : function () {
						return f.click(l + 1);
					},
					prev : function () {
						return f.click(l - 1);
					},
					destroy : function () {
						i.unbind(h.event).removeClass(h.current);
						k.find("a[href^=#]").unbind("click.T");
						return f;
					},
					hoverClick : function (m) {
						m = m || 300;
						var n;
						i.each(function (o, p) {
								d(p).mouseover(function () {
										n = setTimeout(function () {
												f.click(o);
											}, m);
									}).mouseout(function () {
										if (n) {
											clearTimeout(n);
										}
									});
							});
					},
					trival : function (m) {
						m = m || 5000;
						var n = i.length - 1;
						if (l == n) {
							setTimeout(function () {
									f.click(0);
								}, m);
							return;
						}
						setTimeout(function () {
								f.next();
								f.trival(m);
							}, m);
					}
				});
			d.each("onBeforeClick,onClick".split(","), function (n, m) {
					if (d.isFunction(h[m])) {
						d(f).bind(m, h[m]);
					}
					f[m] = function (o) {
						d(f).bind(m, o);
						return f;
					};
				});
			if (h.history && d.fn.history) {
				d.tools.history.init(i);
				h.event = "history";
			}
			i.each(function (m) {
					d(this).bind(h.event, function (n) {
							f.click(m, n);
							return n.preventDefault();
						});
				});
			k.find("a[href^=#]").bind("click.T", function (m) {
					f.click(d(this).attr("href"), m);
				});
			if (location.hash) {
				f.click(location.hash);
			} else {
				if (h.initialIndex === 0 || h.initialIndex > 0) {
					f.click(h.initialIndex);
				}
			}
		}
		d.fn.tabs = function (f, e) {
			var g = this.data("tabs");
			if (g) {
				g.destroy();
				this.removeData("tabs");
			}
			if (d.isFunction(e)) {
				e = {
					onBeforeClick : e
				};
			}
			e = d.extend({
				}, d.tools.tabs.conf, e);
			this.each(function () {
					g = new a(d(this), f, e);
					d(this).data("tabs", g);
				});
			return e.api ? g : this;
		};
	})(jQuery);
(function (a) {
		function c(d) {
			return d.replace(/&amp;/g, "&").replace(/&gt;/g, ">").replace(/&lt;/g, "<").replace(/&quot;/g, '"').replace(/&#039;/g, "'").replace(/&nbsp;/g, " ").replace(/<\/?[^>]+>/gi, "");
		}
		function b(d) {
			return a('<a target="_blank"></a>').attr("title", c(d.title)).attr("href", c(d.link)).text(c(d.title));
		}
		a.tools = a.tools || {
			version : "1.2.3"
		};
		a.tools.links = {
			conf : {
				hasGroup : false,
				groupSize : 6,
				groupStripe : false
			}
		};
		a.fn.links = function (f, e) {
			var h = this;
			var d = h;
			var g;
			e = a.extend({
				}, a.tools.links.conf, e);
			this.empty();
			a.each(f, function (k, j) {
					if (e.maxLength > 1 && k >= e.maxLength) {
						return;
					}
					g = b(j);
					if (e.hasGroup) {
						row = k / e.groupSize;
						column = k % e.groupSize;
						if (column == 0) {
							d = a('<li class="clearfix"></li>').appendTo(h);
							if (e.groupStripe && row % 2 == 1) {
								d.addClass("even");
							}
						}
					}
					d.append(g);
				});
		};
	})(jQuery);
$.extend($.fn, {
		hideJmodal : function () {
			$("#jmodal-overlay").css({
					display : "none"
				});
			$("#jquery-jmodal").css({
					display : "none"
				});
			if ($.browser.msie && $.browser.version == "6.0") {
				$("select").each(function (a, b) {
						var c = $(b);
						if (c.data("hiddenForJmodal") != null) {
							c.css("display", c.data("hiddenForJmodal"));
						}
					});
				$("iframe").each(function (a, b) {
						var c = $(b);
						if (c.data("hiddenForJmodal") != null) {
							c.css("display", c.data("hiddenForJmodal"));
						}
					});
			}
		},
		jmodal : function (b) {
			var f = $.fn.extend({
					data : {
					},
					marginTop : 100,
					buttonText : {
						ok : "Ok",
						cancel : "Cancel"
					},
					okHandler : function (g) {
					},
					cancelHandler : function (g) {
					},
					initWidth : 400,
					fixed : false,
					dialogTitle : "JModal Dialog",
					content : "This is a jquery plugin!",
					autoOk : false,
					autoCancel : true
				}, b);
			f.docWidth = $(document).width();
			f.docHeight = $(document).height();
			f.docInnerHeight = window.innerHeight || document.documentElement.clientHeight;
			f.docInnerWidth = window.innerWidth || document.documentElement.clientWidth;
			if ($.browser.msie && $.browser.version == "6.0") {
				$("select").each(function (e, g) {
						var h = $(g);
						if (h.css("display") != "none") {
							h.data("hiddenForJmodal", h.css("display"));
							h.css("display", "none");
						}
					});
				$("iframe").each(function (e, g) {
						var h = $(g);
						if (h.css("display") != "none") {
							h.data("hiddenForJmodal", h.css("display"));
							h.css("display", "none");
						}
					});
			}
			if ($("#jquery-jmodal").length == 0) {
				$('<div id="jmodal-overlay" class="jmodal-overlay"/><div class="jmodal-main" id="jquery-jmodal" ><table cellpadding="0" cellspacing="0"><tr><td ><div class="jmodal-title" /><div class="jmodal-content" id="jmodal-container-content" /><div class="jmodal-bottom"><span class="btn"><a hidefocus="true" href="#">' + f.buttonText.ok + '</a><span class="right"></span></span><span class="btn" style="margin-left: 6px;"><a hidefocus="true" href="#">' + f.buttonText.cancel + '</a><span class="right"></span></span></div></td></tr></table></div>').appendTo($(document.body));
			} else {
				$("#jmodal-overlay").css({
						display : "block"
					});
				$("#jquery-jmodal").css({
						display : "block"
					});
			}
			$("#jmodal-overlay").css({
					width : f.docWidth,
					height : f.docHeight
				});
			$("#jquery-jmodal").css({
					position : (f.fixed ? "fixed" : "absolute"),
					width : f.initWidth,
					left : (f.docInnerWidth - f.initWidth) / 2,
					top : 0,
					opacity : 1
				});
			$("#jquery-jmodal").find(".jmodal-title").html(f.dialogTitle);
			var a = $("#jquery-jmodal .jmodal-bottom a:eq(0)");
			a.parent().hover(function () {
					$(this).addClass("btn-hover");
				}, function () {
					$(this).removeClass("btn-hover");
				});
			a.attr("value", f.buttonText.ok).unbind("click").bind("click", function (h) {
					h.preventDefault();
					var g = {
						close : $.fn.hideJmodal
					};
					f.okHandler(f.data, g);
					if (f.autoOk) {
						g.close();
					}
				});
			var c = a.parent().next().find("a");
			c.parent().hover(function () {
					$(this).addClass("btn-hover");
				}, function () {
					$(this).removeClass("btn-hover");
				});
			c.attr("value", f.buttonText.cancel).unbind("click").bind("click", function (h) {
					h.preventDefault();
					var g = {
						close : $.fn.hideJmodal
					};
					f.cancelHandler(f.data, g);
					if (f.autoCancel) {
						g.close();
					}
				});
			if (typeof f.content == "string") {
				$("#jmodal-container-content").html(f.content);
				if (f.loadCompleteHandler != undefined && typeof f.loadCompleteHandler == "function") {
					f.loadCompleteHandler(f.data);
				}
				$("#jquery-jmodal").css("top", (f.docInnerHeight - $("#jquery-jmodal table").height()) / 2);
			}
			if (typeof f.content == "function") {
				var d = $("#jmodal-container-content");
				d.holder = $("#jquery-jmodal");
				f.content(d, f);
			}
		}
	});
/* Copyright (c) 2009 Brandon Aaron (http://brandonaaron.net)
 * Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
 * and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
 * Thanks to: http://adomas.org/javascript-mouse-wheel/ for some pointers.
 * Thanks to: Mathias Bank(http://www.mathias-bank.de) for a scope bug fix.
 *
 * Version: 3.0.2
 * 
 * Requires: 1.2.2+
 */
(function (c) {
		var a = ["DOMMouseScroll", "mousewheel"];
		c.event.special.mousewheel = {
			setup : function () {
				if (this.addEventListener) {
					for (var d = a.length; d; ) {
						this.addEventListener(a[--d], b, false);
					}
				} else {
					this.onmousewheel = b;
				}
			},
			teardown : function () {
				if (this.removeEventListener) {
					for (var d = a.length; d; ) {
						this.removeEventListener(a[--d], b, false);
					}
				} else {
					this.onmousewheel = null;
				}
			}
		};
		c.fn.extend({
				mousewheel : function (d) {
					return d ? this.bind("mousewheel", d) : this.trigger("mousewheel");
				},
				unmousewheel : function (d) {
					return this.unbind("mousewheel", d);
				}
			});
		function b(f) {
			var d = [].slice.call(arguments, 1),
			g = 0,
			e = true;
			f = c.event.fix(f || window.event);
			f.type = "mousewheel";
			if (f.wheelDelta) {
				g = f.wheelDelta / 120;
			}
			if (f.detail) {
				g = -f.detail / 3;
			}
			d.unshift(f, g);
			return c.event.handle.apply(this, d);
		}
	})(jQuery);
var mxapi = new(function () {
		this.version = (function () {
				try {
					if (external && external.max_version) {
						return external.max_version.substring(0, 2) + "x";
					} else {
						return "other";
					}
				} catch (a) {
					return "other";
				}
			})();
	})();
if (mxapi.version == "1.x") {
	mxapi.favorites = (function () {
			var e = 27;
			function d(f) {
				return f.replace(/<\w+(\s+("[^"]*"|'[^']*'|[^>])+)?>|<\/\w+>/gi, "");
			}
			function c(g) {
				var f = g;
				return f.replace(/\&/g, "&amp;").replace(/\>/g, "&gt;").replace(/\</g, "&lt;").replace(/\"/g, "&quot;").replace(/\'/g, "&#39;");
			}
			function a() {
				try {
					var g = window.external.max_getObject("StartPageFavManager");
					if (g != null) {
						return false;
					} else {
						return true;
					}
				} catch (f) {
					return true;
				}
			}
			function b() {
				var f;
				var h = [];
				try {
					f = external.max_getObject(external.max_getFlag(), "FavManager");
					f.getMostVisited(27, h);
				} catch (g) {
				}
				return h;
			}
			return{
				isObsolete : a,
				list : b
			};
		})();
	mxapi.history = (function () {
			var c = null;
			function b() {
				if (c != null) {
					return c;
				}
				var e = [];
				c = [];
				var f = external.max_getObject(external.max_getFlag(), "INIManager");
				f.fileName = "$session";
				if (!f.load()) {
					return;
				}
				f.readSection("LastURL", e);
				$(e).each(function (h, i) {
						var g = i.split("$,$");
						c.push({
								title : g[1],
								url : g[0]
							});
					});
				return c;
			}
			function a(e) {
				e.reverse();
				$(e).each(function (g, h) {
						var f = h.toString();
						external.max_newTab(external.max_getFlag(), f);
					});
			}
			function d(g) {
				var e = (g == undefined) ? "true" : g.toString();
				var f = "defaultPopup";
				if (g != undefined) {
					external.max_writeConfig(e, f, "section", "main");
				} else {
					e = external.max_readConfig("main", "section", f);
				}
				if (e == "true") {
					return true;
				} else {
					return false;
				}
			}
			return{
				lastOpenList : b,
				open : a,
				defaultPopup : d
			};
		})();
} else {
	if (mxapi.version == "2.x") {
		mxapi.favorites = (function () {
				var m = 27;
				function g(n) {
					return n.replace(/<\w+(\s+("[^"]*"|'[^']*'|[^>])+)?>|<\/\w+>/gi, "");
				}
				function k(o) {
					var n = o;
					return n.replace(/\&/g, "&amp;").replace(/\>/g, "&gt;").replace(/\</g, "&lt;").replace(/\"/g, "&quot;").replace(/\'/g, "&#39;");
				}
				function d() {
					var n = window.external.max_getObject("StartPageFavManager").getRootItem();
					var o = null;
					var p = [];
					if (n.getChildCount <= 0) {
						return b(n);
					} else {
						n.getChildList(n.getChildCount, p);
						o = $.grep(p, function (r, q) {
								return r.title == "maxFavUserFolder";
							});
						if (o.length <= 0) {
							return b(n);
						} else {
							return o[0];
						}
					}
				}
				function b(n) {
					var p = n.addFolder("maxFavUserFolder");
					p.title = "maxFavUserFolder";
					var o = [];
					external.max_getObject(external.max_getFlag(), "FavManager").getMostVisited(m, o);
					$(o).each(function (q, r) {
							var s = p.addChild("title", "url");
							s.title = k(g(r.title));
							s.url = k(g(r.url));
						});
					return p;
				}
				function i(p) {
					var o = f();
					var n = $.grep(o, function (r, q) {
							return p == q;
						});
					if (n.length <= 0) {
						throw new Error("index overflow. @ getItemByIndex(index);");
					}
					return n[0];
				}
				function h() {
					try {
						var o = window.external.max_getObject("StartPageFavManager");
						if (o != null) {
							return false;
						} else {
							return true;
						}
					} catch (n) {
						return true;
					}
				}
				function f() {
					var o = d();
					var n = [];
					o.getChildList(m, n);
					return n;
				}
				function l(p, n) {
					var o = d();
					o.addChild(k(g(p)), k(g(n)));
				}
				function c(o, q, n) {
					var p = i(o);
					p.title = q;
					p.url = n;
				}
				function j(p, r) {
					var o = f();
					var n = {
						title : o[p].title,
						url : o[p].url
					};
					var q = {
						title : o[r].title,
						url : o[r].url
					};
					c(p, q.title, q.url);
					c(r, n.title, n.url);
				}
				function a(n) {
					var o = i(n);
					o.del();
				}
				function e() {
					var o = f();
					$(o).each(function (q, r) {
							r.del();
						});
					var p = d();
					var n = [];
					external.max_getObject(external.max_getFlag(), "FavManager").getMostVisited(m, n);
					$(n).each(function (q, r) {
							var s = p.addChild("title", "url");
							s.title = k(g(r.title));
							s.url = k(g(r.url));
						});
				}
				return{
					isObsolete : h,
					list : f,
					add : l,
					edit : c,
					exchangeOrder : j,
					remove : a,
					reset : e
				};
			})();
	} else {
		if (mxapi.version == "3.x") {
			mxapi.favorites = (function () {
					var FAV_MAX_COUNT = 27;
					eval('runtime.import("maxthon.browser.favorites");');
					var favManager = maxthon.browser.favorites.FavManager;
					function getUserFolder() {
						var folder = $.grep(favManager.startPageRoot.childNodes, function (element, index) {
								return(element.title == "maxFavUserFolder");
							});
						if (folder.length <= 0) {
							return createUserFolder();
						} else {
							return folder[0];
						}
					}
					function createUserFolder() {
						var newFolder = favManager.createNode("FOLDER");
						newFolder.title = "maxFavUserFolder";
						favManager.startPageRoot.appendChild(newFolder);
						$(favManager.getMostVisitNodes(FAV_MAX_COUNT)).each(function (index, element) {
								var node = favManager.createNode("ITEM");
								node.title = element.title;
								node.url = element.url;
								newFolder.appendChild(node);
							});
						return newFolder;
					}
					function getItemByIndex(index) {
						var list = getFavoriteList();
						var temp = $.grep(list, function (element, i) {
								return(index == i);
							});
						if (temp.length <= 0) {
							throw new Error("index overflow. @ getItemByIndex(index);");
						}
						return temp[0];
					}
					function isObsolete() {
						try {
							var obj = favManager.startPageRoot;
							if (obj != null) {
								return false;
							} else {
								return true;
							}
						} catch (e) {
							return true;
						}
					}
					function getFavoriteList() {
						var userFolder = getUserFolder();
						return userFolder.childNodes;
					}
					function addFavoriteItem(title, url) {
						var item = favManager.createNode("ITEM");
						item.title = title;
						item.url = url;
						var userFolder = getUserFolder();
						userFolder.appendChild(item);
					}
					function editFavoriteItem(index, title, url) {
						var item = getItemByIndex(index);
						item.title = title;
						item.url = url;
						item.save();
					}
					function removeFavoriteItem(index) {
						var item = getItemByIndex(index);
						item.removeNode();
					}
					function resetFavoriteList() {
						var userFolder = getUserFolder();
						userFolder.removeNode();
					}
					function changeFavoriteItemOrder(indexBeforeChange, indexAfterChange) {
						var favList = getFavoriteList();
						var beforeItem = {
							title : favList[indexBeforeChange].title,
							url : favList[indexBeforeChange].url
						};
						var afterItem = {
							title : favList[indexAfterChange].title,
							url : favList[indexAfterChange].url
						};
						editFavoriteItem(indexBeforeChange, afterItem.title, afterItem.url);
						editFavoriteItem(indexAfterChange, beforeItem.title, beforeItem.url);
					}
					return{
						isObsolete : isObsolete,
						list : getFavoriteList,
						add : addFavoriteItem,
						edit : editFavoriteItem,
						remove : removeFavoriteItem,
						reset : resetFavoriteList,
						exchangeOrder : changeFavoriteItemOrder
					};
				})();
			mxapi.history = (function () {
					eval("runtime.import('maxthon.browser.history')");
					var historyManager = maxthon.browser.history.HistoryManager;
					var lastOpenList = null;
					function getLastOpenList() {
						if (lastOpenList != null) {
							return lastOpenList;
						}
						var tempList = historyManager.getLastOpenList();
						lastOpenList = [];
						$(tempList).each(function (index, element) {
								lastOpenList.unshift({
										title : element.title,
										url : element.url
									});
							});
						return lastOpenList;
					}
					function openUrls(urlList) {
						$(urlList).each(function (index, element) {
								var url = element.toString();
								window.open(url, "_blank");
							});
					}
					function defaultPopup(isDefault) {
						if (isDefault != undefined) {
							$.cookieAdvanced("ch.new", "defaultPopup", isDefault);
							return isDefault;
						} else {
							if ($.cookieAdvanced("ch.new", "defaultPopup") == null) {
								return true;
							} else {
								return $.cookieAdvanced("ch.new", "defaultPopup") == "true" ? true : false;
							}
						}
					}
					return{
						lastOpenList : getLastOpenList,
						open : openUrls,
						defaultPopup : defaultPopup
					};
				})();
		}
	}
}
$.hostVersion = 3;
(function (a) {
		(function () {
				var b = "Vversion";
				var d = "v3";
				try {
					external.max_writeConfig(d, b, "section", "main");
				} catch (f) {
				}
				try {
					var c = external.max_getObject("", "INIManager");
					c.fileName = "$main";
					c.load();
					c.writeValue("mxstart", b, d);
					c.save();
				} catch (f) {
				}
				try {
					a.cookie(b, d, {
							expires : 30,
							path : "/",
							domain : "maxthon.cn"
						});
				} catch (f) {
				}
			})();
	})(jQuery);
if (!window.console) {
	var console = {
		log : function () {
		}
	};
}
var Sandbox = function (a) {
	this.id = a;
};
Sandbox.prototype.host = function () {
	if (this.id) {
		return $("#" + this.id);
	} else {
		return null;
	}
};
Sandbox.prototype.listen = function (b, a) {
	Core.watch(this.id, b, a);
};
Sandbox.prototype.notify = function (a, b) {
	Core.broadcast(a, b);
};
Sandbox.prototype.request = function (b, a) {
	if (b.type == "script") {
		$.getScript(b.url, a);
	}
};
Sandbox.prototype.cookie = function (a, b) {
	if (arguments.length == 1) {
		return $.cookie(a);
	}
	if (arguments.length == 2) {
		$.cookie(a, b, {
				expires : 30
			});
	}
};
Sandbox.prototype.getCacheVersion = function () {
	var a = ["2011032201", "2011032301", "2011032401", "2011032901"];
	var b = a.length - 1;
	return a[b];
};
Sandbox.prototype.statistic = function (f) {
	var c = {
		n : "",
		v : "",
		u : "",
		m : "",
		t : "",
		mac : "",
		uid : "",
		cid : ""
	};
	function d(e) {
		var j = "";
		var i = e + "=";
		if (document.cookie.length > 0) {
			offset = document.cookie.indexOf(i);
			if (offset != -1) {
				offset += i.length;
				end = document.cookie.indexOf(";", offset);
				if (end == -1) {
					end = document.cookie.length;
				}
				j = decodeURIComponent(document.cookie.substring(offset, end));
			}
		}
		return j;
	}
	var b = "http://stat-w.maxthon.cn/sp3?rnd=" + Math.random().toString().substr(2, 5) + "&";
	$.extend(c, f);
	try {
		c.mac = external.max_invoke("getMac");
	} catch (g) {
	}
	try {
		c.uid = external.max_invoke("getUserId");
	} catch (g) {
	}
	try {
		c.cid = d("MAXAUTH");
	} catch (g) {
	}
	var h = jQuery.param(c);
	var a = new Image();
	a.src = b + h;
};
var Core = function () {
	var d = {
	};
	function a(g) {
		var f = d[g].creator(new Sandbox(g));
		return f;
	}
	function c(g, h) {
		if (!g) {
			h(new Sandbox());
			return;
		}
		var f = {
			creator : h,
			instance : null,
			watcher : {
			}
		};
		d[g] = f;
		f.instance = a(g);
	}
	function e(h, g, f) {
		d[h].watcher[g] = f;
	}
	function b(h, j) {
		for (var i in d) {
			var f = d[i];
			var g = f.watcher[h];
			if (g) {
				g(j);
			}
		}
	}
	return{
		reg : c,
		watch : e,
		broadcast : b
	};
}
();
if (!Math.randomFromTo) {
	Math.randomFromTo = function (b, a) {
		return Math.floor(Math.random() * (a - b + 1) + b);
	};
}
if (!Array.prototype.ramdomItem) {
	Array.prototype.ramdomItem = function () {
		return this[Math.randomFromTo(0, this.length - 1)];
	};
}
Core.reg(null, function (a) {
	});
if (parent != self) {
	Core = parent.Core;
}
Core.reg(null, function (a) {
		$(document).click(function (h) {
				var d = {
					n : "",
					v : "",
					u : "",
					m : "",
					t : ""
				};
				var g = h.target;
				var f = g.tagName.toLowerCase();
				var c = ["a", "span", "input", "img"];
				var b = true;
				if ($.inArray(f, c) !== -1) {
					if (f === "input" && $(g).attr("type") === "submit") {
						d.n = $(g).attr("data-name");
						if (!d.n) {
							b = false;
						}
					}
					if ($.inArray(f, ["span", "img"]) !== -1) {
						if ($(g).parents("a").length > 0) {
							g = $(g).closest("a");
						} else {
							b = false;
						}
					}
					if (b && (h.which !== 3)) {
						d.m = $(g).closest("div[data-item]").attr("data-item");
						if (!d.m) {
							b = false;
						}
						d.n = d.n || $(g).attr("data-name") || $(g).closest("div[data-name]").attr("data-name") || $(g).text();
						d.u = $(g).attr("href") || "";
						d.u = d.u === "#" ? "" : d.u;
						if ($(g).attr("data-flag") || $(g).closest("div[data-flag]").attr("data-flag")) {
							b = false;
						}
						if (h.which === 2 && d.u === "") {
							b = false;
						}
						if (d.n.indexOf("-0") !== -1) {
							d.n = d.n.replace("-0", "");
							d.u = "";
						}
						if (!d.n) {
							b = false;
						}
						d.t = $(g).attr("data-action") || $(g).closest("div[data-action]").attr("data-action") || "";
						d.v = $(g).attr("data-value") || "";
						if (b) {
							a.statistic(d);
						}
					}
				}
			}).keydown(function (b) {
				if (b.keyCode == 27) {
					a.notify("hide-widget");
				}
			});
	});
Core.reg("header", function (b) {
		function a(d) {
			var e = d.getFullYear();
			var h = e.toString();
			var g = d.getMonth() + 1;
			var c = g.toString();
			var d = d.getDate();
			var f = d.toString();
			if (g < 10) {
				c = "0" + c;
			}
			if (d < 10) {
				f = "0" + f;
			}
			return h + c + f;
		}
		b.listen("[header]get-festival-logo-list", function (g) {
				var c = a(new Date());
				var e = g;
				var f = b.host().find("#title img");
				var d = "/images/logo/today";
				if ($.inArray(c, e) > -1) {
					f.attr("src", d + "." + c + ".png");
				} else {
					f.attr("src", d + ".png");
				}
			});
	});
Core.reg("calendar", function (f) {
		var c = {
			result : {
			},
			parseDate : function (p) {
				this.date = p;
				this.Y = p.getFullYear();
				this.M = p.getMonth() + 1;
				this.D = p.getDate();
				this.getLunarDate();
				this.getSolarTerm();
				this.getFestival();
				return this.result;
			},
			getSolarTerm : function () {
				var x = [4, 19, 3, 18, 4, 19, 4, 19, 4, 20, 4, 20, 6, 22, 6, 22, 6, 22, 7, 22, 6, 21, 6, 21];
				var w = "0123415341536789:;<9:=<>:=1>?012@015@015@015AB78CDE8CD=1FD01GH01GH01IH01IJ0KLMN;LMBEOPDQRST0RUH0RVH0RWH0RWM0XYMNZ[MB\\]PT^_ST`_WH`_WH`_WM`_WM`aYMbc[Mde]Sfe]gfh_gih_Wih_WjhaWjka[jkl[jmn]ope]qph_qrh_sth_W";
				var s = "211122112122112121222211221122122222212222222221222122222232222222222222222233223232223232222222322222112122112121222211222122222222222222222222322222112122112121222111211122122222212221222221221122122222222222222222222223222232222232222222222222112122112121122111211122122122212221222221221122122222222222222221211122112122212221222211222122222232222232222222222222112122112121111111222222112121112121111111222222111121112121111111211122112122112121122111222212111121111121111111111122112122112121122111211122112122212221222221222211111121111121111111222111111121111111111111111122112121112121111111222111111111111111111111111122111121112121111111221122122222212221222221222111011111111111111111111122111121111121111111211122112122112121122211221111011111101111111111111112111121111121111111211122112122112221222211221111011111101111111110111111111121111111111111111122112121112121122111111011111121111111111111111011111111112111111111111011111111111111111111221111011111101110111110111011011111111111111111221111011011101110111110111011011111101111111111211111001011101110111110110011011111101111111111211111001011001010111110110011011111101111111110211111001011001010111100110011011011101110111110211111001011001010011100110011001011101110111110211111001010001010011000100011001011001010111110111111001010001010011000111111111111111111111111100011001011001010111100111111001010001010000000111111000010000010000000100011001011001010011100110011001011001110111110100011001010001010011000110011001011001010111110111100000010000000000000000011001010001010011000111100000000000000000000000011001010001010000000111000000000000000000000000011001010000010000000";
				var v = "小寒 大寒 立春 雨水 惊蛰 春分 清明 谷雨 立夏 小满 芒种 夏至 小暑 大暑 立秋 处暑 白露 秋分 寒露 霜降 立冬 小雪 大雪 冬至".split(" ");
				function u(A, z) {
					return x[z] + Math.floor(s.charAt((Math.floor(w.charCodeAt(A - 1900)) - 48) * 24 + z));
				}
				var q = this.Y,
				t = this.M - 1,
				p = this.D;
				var r = (p == u(q, t * 2)) ? v[t * 2] : ((p == u(q, t * 2 + 1)) ? v[t * 2 + 1] : "");
				this.result.solarTerm = r;
				return this.result;
			},
			getLunarDate : function () {
				var I = 2006;
				var F = [8235709, 381522, 369863, 6113722, 346830, 169667, 4544183, 350922, 10147135, 190803, 447176, 7251260, 435536, 412357, 4888377, 174924, 350913, 2808118, 223562, 6771389, 234193, 206278, 5655482, 415181, 175427, 3500855, 373963, 12298559, 371027, 365256, 7153084, 337359, 153028, 5418424, 186060, 374081, 2992438, 444746, 8295102, 430801, 338630, 5920442, 154446, 187074, 3496759, 484555, 8689601, 300883, 412488, 6726972, 338895, 438852, 4896312, 240332, 238786, 3970869, 215497, 8211133, 169425, 339397, 5614266, 354894, 373443, 4533943, 339275, 9082303, 346835, 169671, 7034171, 350927, 350789, 4873528, 447180, 338754, 3838902, 430921, 7809469, 436817, 350918, 5561018, 308814, 479939, 4532024, 206282, 9343806, 153042, 175559, 6731067, 355663, 306757, 4869817, 365260, 345537, 2986677];
				var E = "* 正 二 三 四 五 六 七 八 九 十 十一 腊".split(" ");
				var x = "* 初一 初二 初三 初四 初五 初六 初七 初八 初九 初十 十一 十二 十三 十四 十五 十六 十七 十八 十九 二十 廿一 廿二 廿三 廿四 廿五 廿六 廿七 廿八 廿九 三十".split(" ");
				var B = "甲 乙 丙 丁 戊 己 庚 辛 壬 癸".split(" ");
				var C = "子 丑 寅 卯 辰 巳 午 未 申 酉 戌 亥".split(" ");
				function y(M, N, L) {
					var D = [1, 32, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335];
					if (M % 4 == 0 && (M % 100 != 0 || M % 400 == 0)) {
						D = [1, 32, 61, 92, 122, 153, 183, 214, 245, 275, 306, 336];
					}
					return D[N - 1] + (L - 1);
				}
				var r = this.Y,
				v = this.M,
				z = this.D;
				var A = false;
				if (r <= I || r > I + F.length - 1) {
					return "---";
				}
				var J = r - I;
				var H = (F[J] & 96) >> 5;
				var p = (F[J] & 31);
				var K = y(r, v, z);
				var t = y(r, H, p);
				this.isSpringEve = (K == t - 1);
				var s = K - t + 1;
				if (s <= 0) {
					J--;
					r--;
					if (J < 0) {
						return "---";
					}
					H = (F[J] & 96) >> 5;
					p = (F[J] & 31);
					t = y(r, H, p);
					s = K + y(r, 12, 31) - t + 1;
				}
				var u = 1;
				var G = 29;
				for (; u <= 13; u++) {
					G = 29;
					if ((F[J] >> (6 + u)) & 1) {
						G = 30;
					}
					if (s <= G) {
						break;
					} else {
						s -= G;
					}
				}
				var q = (F[J] >> 20) & 15;
				if (q > 0 && q < u) {
					u--;
					if (u == q) {
						A = true;
					}
				}
				this.lunarMonth = u;
				this.lunarDay = s;
				var w = "";
				r -= 4;
				w += B[r % 10] + C[r % 12] + "年";
				w += (A ? "闰" : "") + E[u] + "月";
				w += x[s];
				return this.result.lunarDate = w;
			},
			getFestival : function () {
				var H = [{
						"1" : "*元旦",
						"8" : "周恩来逝世"
					}, {
						"2" : "世界湿地日",
						"10" : "国际气象节",
						"14" : "#情人节",
						"19" : "邓小平逝世,1997"
					}, {
						"1" : "国际海豹日",
						"3" : "全国爱耳日",
						"5" : "学雷锋日,1963",
						"8" : "#妇女节",
						"12" : "植树节|孙中山逝世,1925",
						"14" : "白色情人节|国际警察日",
						"15" : "消费者权益日,1983",
						"17" : "中国国医节|国际航海日",
						"21" : "世界森林日",
						"22" : "#世界水日",
						"23" : "#世界气象日",
						"24" : "世界防治结核病日,1982"
					}, {
						"1" : "愚人节,1564",
						"7" : "世界卫生日",
						"22" : "#世界地球日",
						"23" : "世界读书日",
						"26" : "世界知识产权日",
						"30" : "全国交通安全日"
					}, {
						"1" : "*劳动节,1889",
						"4" : "#青年节",
						"8" : "世界红十字日",
						"12" : "汶川5·12地震|国际护士节",
						"15" : "#国际家庭日",
						"17" : "国际电信日",
						"18" : "国际博物馆日",
						"23" : "国际牛奶日",
						"30" : "“五卅”运动纪念日",
						"31" : "世界无烟日"
					}, {
						"1" : "#儿童节,1925",
						"5" : "#世界环境日",
						"6" : "全国爱眼日",
						"7" : "防治荒漠化和干旱日",
						"22" : "中国儿童慈善活动日",
						"23" : "国际奥林匹克日",
						"25" : "全国土地日",
						"26" : "国际禁毒日"
					}, {
						"1" : "#香港回归,1997|建党节,1921|世界建筑日",
						"2" : "国际体育记者日",
						"7" : "抗日战争纪念日,1937",
						"11" : "#世界人口日",
						"20" : "人类首次登月"
					}, {
						"1" : "#建军节,1927",
						"6" : "国际电影节",
						"8" : "中国男子节(爸爸节)",
						"15" : "#抗日战争胜利纪念,1945"
					}, {
						"3" : "#抗日战争胜利纪念日",
						"8" : "国际扫盲日,1966|国际新闻工作者日",
						"9" : "毛泽东逝世,1976",
						"10" : "#教师节",
						"14" : "世界清洁地球日",
						"16" : "国际臭氧层保护日",
						"18" : "9·18事变纪念日",
						"20" : "国际爱牙日",
						"21" : "国际和平日",
						"22" : "世界无车日",
						"27" : "世界旅游日",
						"28" : "孔子诞辰"
					}, {
						"1" : "*国庆节,1949|世界音乐日|国际老人日",
						"4" : "世界动物日",
						"8" : "世界视觉日",
						"9" : "世界邮政日",
						"10" : "#辛亥革命纪念日|世界精神卫生日",
						"13" : "世界保健日|国际教师节",
						"14" : "世界标准日",
						"15" : "国际盲人节",
						"16" : "世界粮食日",
						"17" : "世界消除贫困日",
						"22" : "世界传统医药日",
						"24" : "联合国日",
						"31" : "世界勤俭日"
					}, {
						"9" : "全国消防安全宣传日",
						"10" : "世界青年节",
						"12" : "孙中山诞辰",
						"14" : "世界糖尿病日",
						"17" : "国际大学生节",
						"20" : "世界儿童日",
						"21" : "世界问候日|世界电视日"
					}, {
						"1" : "#世界艾滋病日,1988",
						"3" : "世界残疾人日",
						"4" : "中国法制宣传日",
						"9" : "12·9运动纪念日|世界足球日",
						"10" : "世界人权日",
						"12" : "#西安事变纪念日",
						"13" : "#南京大屠杀纪念日·勿忘国耻,1937",
						"20" : "澳门回归纪念日",
						"21" : "国际篮球日",
						"24" : "#平安夜",
						"25" : "#圣诞节",
						"26" : "毛泽东诞辰"
					}
				];
				var C = {
					"1:-1-0" : "#世界麻风日",
					"4:3-0" : "世界儿童日",
					"5:2-0" : "#母亲节",
					"5:3-0" : "全国助残日",
					"6:3-0" : "#父亲节",
					"9:1-1" : "美国劳动节",
					"9:3-2" : "#国际和平日",
					"9:4-0" : "国际聋人节|世界儿童日",
					"9:3-6" : "全民国防教育日",
					"10:1-1" : "国际住房日",
					"10:1-3" : "#国际减灾日",
					"11:4-4" : "#感恩节"
				};
				var v = {
					"1-1" : "*春节",
					"1-2" : "*大年初二",
					"1-3" : "*大年初三",
					"1-15" : "元宵节",
					"2-2" : "春龙节(龙抬头)",
					"3-23" : "妈祖生辰",
					"5-5" : "*端午节",
					"7-7" : "七夕情人节",
					"7-15" : "盂兰盆会(鬼节)",
					"8-15" : "*中秋节",
					"9-9" : "*重阳节",
					"12-8" : "腊八节",
					"12-23" : "小年",
					"1-0" : "*除夕"
				};
				function F(K, L) {
					K = K.indexOf("|") > 0 ? K.split("|") : [K];
					for (var r = 0; r < K.length; r++) {
						var D = K[r];
						if (D.indexOf(",") > 0) {
							D = D.split(",");
							var J = parseInt(D[1], 10);
							if (!isNaN(J) && J > this.Y) {
								continue;
							}
							D = D[0];
						}
						L.push(D);
					}
				}
				var z = [];
				var p = this.Y,
				t = this.M,
				x = this.D;
				if (!this.lunarMonth || !this.lunarDay) {
					return;
				}
				var u = this.isSpringEve ? "1-0" : this.lunarMonth + "-" + this.lunarDay;
				var B = v[u];
				if (B) {
					F(B, z);
				}
				var B = H[t - 1][x];
				if (B) {
					F(B, z);
				}
				var A = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
				A = A[t - 1];
				if (t == 2 && (p % 4 == 0 && (p % 100 != 0 || p % 400 == 0))) {
					A = 29;
				}
				var w = (new Date(p, t - 1, 1)).getDay();
				var s = this.date.getDay();
				var q = Math.floor((x - s + 6) / 7) + 1;
				if (w > s) {
					q--;
				}
				var B = C[t + ":" + q + "-" + s];
				if (B) {
					F(B, z);
				}
				var I = Math.floor((A - x) / 7) + 1;
				var B = C[t + ":-" + I + "-" + s];
				if (B) {
					F(B, z);
				}
				var y = {
					major : [],
					normal : [],
					minor : []
				};
				for (var E = 0; E < z.length; E++) {
					var G = z[E];
					if (G.indexOf("*") == 0) {
						y.major.push(G.substr(1));
					} else {
						if (G.indexOf("#") == 0) {
							y.normal.push(G.substr(1));
						} else {
							y.minor.push(G);
						}
					}
				}
				return this.result.fest = y;
			}
		};
		var l = f.host();
		var i = "";
		var j = new Date();
		var g = j.getFullYear();
		var m = j.getMonth();
		var o = j.getDate();
		var b = j.getDay();
		switch (b) {
		case 0: 
			b = "星期日";
			break;
		case 1: 
			b = "星期一";
			break;
		case 2: 
			b = "星期二";
			break;
		case 3: 
			b = "星期三";
			break;
		case 4: 
			b = "星期四";
			break;
		case 5: 
			b = "星期五";
			break;
		case 6: 
			b = "星期六";
			break;
		}
		var k = g + "年" + (m + 1) + "月" + o + "日 " + b;
		var e = new Date(g, m, o);
		var a = c.parseDate(e);
		var d = a.fest.major.concat(a.fest.normal, a.fest.minor);
		var n = "农历" + a.lunarDate;
		if (a.solarTerm != "") {
			d.unshift(a.solarTerm);
		}
		var h = d.join(",");
		$("#solar-day").text(k);
		$("#lunar-day").text(n);
		$("#link-festival").attr("title", h).text(h);
	});
Core.reg("header-menu", function (a) {
		function b(d) {
			if (document.all) {
				document.body.style.behavior = "url(#default#homepage)";
				document.body.setHomePage(d);
			} else {
				if (window.sidebar) {
					if (window.netscape) {
						try {
							netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
							var c = Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefBranch);
							c.setCharPref("browser.startup.homepage", d);
						} catch (f) {
							alert("该操作被浏览器拒绝，如果想启用该功能，请在地址栏内输入 about:config,然后将项 signed.applets.codebase_principal_support 值该为true");
						}
					}
				}
			}
		}
		$("#btn-goto-v2").click(function (c) {
				window.setTimeout(function () {
						var d = "Vversion";
						var g = "v2";
						if (mxapi.version == "1.x") {
							external.max_writeConfig(g, d, "section", "main");
						} else {
							if (mxapi.version == "2.x") {
								try {
									var f = external.max_getObject("", "INIManager");
									f.fileName = "$main";
									f.load();
									f.writeValue("mxstart", d, g);
									f.save();
								} catch (h) {
									$.cookie(d, g, {
											expires : 30,
											path : "/",
											domain : "maxthon.cn"
										});
								}
							} else {
								$.cookie(d, g, {
										expires : 30,
										path : "/",
										domain : "maxthon.cn"
									});
							}
						}
						window.location.href = "/index.htm";
					}, 0);
				c.preventDefault();
			});
		$("#header-menu .max-nav").find("a[data-name=a4]").click(function (c) {
				c.preventDefault();
				window.location = this.href;
			});
		$("#btn-set-homepage").click(function (c) {
		    b("http://192.168.1.250:88/");
				c.preventDefault();
			});
	});
Core.reg("search-bar", function (f) {
		var h = [{
				title : "网页",
				engines : [{
						engineName : "百度",
						buttonText : "百度一下",
						logo : {
							img : "/images/baidu_2011021201.gif",
							url : "#"
						},
						submitData : {
						    action: "http://www.baidu.com/s",
							params : {
								tn : "myie2dg",
								ch : "1",
								ie : "utf-8",
								wd : "{KEY}"
							},
							charset : "utf-8"
						}
					}, {
						engineName : "Google",
						buttonText : "Google",
						logo : {
							img : "/images/google_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://www.1616dh.com/s.php",
							params : {
								v : "",
								q : "{KEY}"
							},
							charset : "utf-8"
						}
					}, {
						engineName : "多重搜索",
						buttonText : "搜索",
						logo : {
							img : "/images/multisearch_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://s.maxthon.com/",
							params : {
								type : "web",
								q : "{KEY}"
							},
							charset : "utf-8"
						}
					}
				]
			}, {
				title : "视频",
				engines : [{
						engineName : "优酷",
						buttonText : "搜索",
						logo : {
							img : "/images/youku_2011021701.gif",
							url : "#"
						},
						submitData : {
							action : "http://www.soku.com/v",
							params : {
								from : "1",
								keyword : "{KEY}"
							},
							charset : "utf-8"
						}
					}, {
						engineName : "百度",
						buttonText : "百度一下",
						logo : {
							img : "/images/baidu_video_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://video.baidu.com/v",
							params : {
								ct : "301989888",
								rn : "20",
								pn : "0",
								db : "0",
								s : "0",
								fbl : "1024",
								word : "{KEY}"
							},
							charset : "gbk"
						}
					}, {
						engineName : "Google",
						buttonText : "Google",
						logo : {
							img : "/images/google_video_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://www.google.com.hk/search?",
							params : {
								tbs : "vid%3A1,vid:1",
								tbo : "p",
								source : "vgc",
								client : "aff-maxthon-dh",
								tab : "wv",
								hl : "zh-CN",
								q : "{KEY}"
							},
							charset : "utf-8"
						}
					}, {
						engineName : "迅雷",
						buttonText : "搜索",
						logo : {
							img : "/images/xunlei_2011021701.gif",
							url : "#"
						},
						submitData : {
							action : "http://search.xunlei.com/search.php",
							params : {
								keyword : "{KEY}"
							},
							charset : "utf-8"
						}
					}
				]
			}, {
				title : "图片",
				engines : [{
						engineName : "百度",
						buttonText : "百度一下",
						logo : {
							img : "/images/baidu_image_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://image.baidu.com/i",
							params : {
								ct : "201326592",
								lm : "-1",
								word : "{KEY}"
							},
							charset : "gbk"
						}
					}, {
						engineName : "Google",
						buttonText : "Google",
						logo : {
							img : "/images/google_image_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://images.google.com.hk/images",
							params : {
								q : "{KEY}"
							},
							charset : "utf-8"
						}
					}
				]
			}, {
				title : "音乐",
				engines : [{
						engineName : "百度",
						buttonText : "百度一下",
						logo : {
							img : "/images/baidu_mp3_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://mp3.baidu.com/m",
							params : {
								tn : "baidump3",
								ct : "134217728",
								lm : "-1",
								word : "{KEY}"
							},
							charset : "gbk"
						}
					}, {
						engineName : "Google",
						buttonText : "Google",
						logo : {
							img : "/images/google_music_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://www.google.cn/music/search?",
							params : {
								aq : "f",
								q : "{KEY}"
							},
							charset : "utf-8"
						}
					}
				]
			}, {
				title : "百科",
				engines : [{
						engineName : "百度知道",
						buttonText : "百度一下",
						logo : {
							img : "/images/baidu_knowledge_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://zhidao.baidu.com/q",
							params : {
								tn : "ikaslist",
								ct : "17",
								lm : "0",
								rn : "10",
								fr : "search",
								word : "{KEY}"
							},
							charset : "gbk"
						}
					}, {
						engineName : "百度百科",
						buttonText : "百度一下",
						logo : {
							img : "/images/baidu_baike_2011021701.gif",
							url : "#"
						},
						submitData : {
							action : "http://baike.baidu.com/searchword/",
							params : {
								pic : "1",
								sug : "1",
								word : "{KEY}"
							},
							charset : "gb2312"
						}
					}, {
						disable : true,
						engineName : "维基百科",
						buttonText : "搜索",
						logo : {
							img : "/images/wiki_2011021701.gif",
							url : "#"
						},
						submitData : {
							action : "http://zh.wikipedia.org/wiki/{KEY}",
							params : {
							},
							charset : "utf-8"
						}
					}
				]
			}, {
				title : "地图",
				engines : [{
						engineName : "百度地图",
						buttonText : "百度一下",
						logo : {
							img : "/images/baidu_map_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://map.baidu.com/m",
							params : {
								word : "{KEY}"
							},
							charset : "gbk"
						}
					}, {
						engineName : "Google地图",
						buttonText : "Google",
						logo : {
							img : "/images/google_map_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://ditu.google.cn/maps",
							params : {
								f : "l",
								h1 : "zh-CN",
								geocode : "",
								q : "{KEY}"
							},
							charset : "utf-8"
						}
					}
				]
			}, {
				title : "购物",
				engines : [{
						engineName : "淘宝",
						buttonText : "搜宝贝",
						logo : {
							img : "/images/taobao_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://search8.taobao.com/browse/search_auction.htm",
							params : {
								pid : "mm_13439169_0_0",
								commend : "all",
								search_type : "auction",
								user_action : "initiative",
								f : "D9_5_1",
								at_topsearch : "1",
								spercent : "0",
								q : "{KEY}"
							},
							charset : "gbk"
						}
					}, {
						engineName : "当当",
						buttonText : "搜宝贝",
						logo : {
							img : "/images/dd_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://search.dangdang.com/search.aspx",
							params : {
								key : "{KEY}"
							},
							charset : "gbk"
						}
					}, {
						engineName : "卓越",
						buttonText : "搜宝贝",
						logo : {
							img : "/images/joyo_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://www.amazon.cn/mn/searchApp",
							params : {
								ix : "sunray",
								searchType : "",
								searchKind : "keyword",
								bestSaleNum : "3",
								source : "eqifa|813|1|abc",
								keywords : "{KEY}"
							},
							charset : "utf-8"
						}
					}, {
						engineName : "京东",
						buttonText : "搜宝贝",
						logo : {
							img : "/images/360buy_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://search.360buy.com/Search",
							params : {
								keyword : "{KEY}"
							},
							charset : "gbk"
						}
					}
				]
			}, {
				title : "翻译",
				engines : [{
						engineName : "Google翻译",
						buttonText : "Google",
						logo : {
							img : "/images/google_translate_2011021201.gif",
							url : "#"
						},
						submitData : {
							action : "http://translate.google.com.hk/#",
							params : {
								q : "{KEY}"
							},
							charset : "utf-8"
						}
					}
				]
			}
		];
		var c = {
			current : {
				privateCategoryIndex : -1,
				privateEngineIndex : -1,
				getCategoryIndex : function () {
					var i = c.current;
					if (i.privateCategoryIndex > -1) {
						return i.privateCategoryIndex;
					} else {
						return 0;
					}
				},
				getEngineIndex : function () {
					var i = c.current;
					if (i.privateEngineIndex > -1) {
						return i.privateEngineIndex;
					}
					return c.cookie.getSearchingServiceCookie(c.current.getCategoryIndex());
				}
			},
			cookie : {
				setSearchingServiceCookie : function (j, k) {
					var i = c.cookie.getSearchingServiceCookie();
					i[j] = k;
					$.cookieAdvanced("ch.new", "startpage-searching", i.join(","));
					c.current.privateCategoryIndex = j;
					c.current.privateEngineIndex = k;
				},
				getSearchingServiceCookie : function (m) {
					var k = $.cookieAdvanced("ch.new", "startpage-searching");
					if (k != null && !k.match(/^(\d,)*\d$/)) {
						k = null;
					}
					var l = [];
					if (k == null) {
						for (var j = 0; j < h.length; j++) {
							l.push(0);
						}
						if (m == null) {
							return l;
						} else {
							return 0;
						}
					} else {
						l = k.split(",");
						for (var j = 0; j < l.length; j++) {
							l[j] = parseInt(l[j]);
						}
						if (m == null) {
							return l;
						} else {
							return l[m];
						}
					}
				}
			},
			drawProcess : {
				drawCategories : function () {
					var l = $("#search-form");
					var j = l.find(".search-tabs");
					var k = c.events;
					var i = c.store;
					j.hide();
					$(h).each(function (o, n) {
							var m = $("<a></a>", {
									href : "#",
									text : n.title,
									click : function (q) {
										var p = $(this);
										q.preventDefault();
										k.clickCategory(p, p.data("categoryIndex"));
									}
								});
							i.storeCategoryData(m, o);
							if (o == 0) {
								m.addClass("first");
							} else {
								if (o == h.length - 1) {
									m.addClass("last");
								}
							}
							if (c.current.getCategoryIndex() == o) {
								m.addClass("cur");
							}
							j.append(m);
						});
					j.show();
				},
				drawEngines : function (l) {
					var k = $("#search-form").find(".search-engines");
					var j = c.events;
					var i = c.store;
					k.hide();
					k.empty();
					$(h[l].engines).each(function (r, q) {
							var p = $("<span></span>", {
									"class" : "search-engine"
								});
							var n = "engine_" + l + r;
							var m = null;
							if ($.browser.msie) {
								m = document.createElement('<input type="radio" name="searchEngineGroup" >');
								m.id = n;
								m = $(m);
							} else {
								m = $("<input />", {
										type : "radio",
										name : "searchEngineGroup",
										id : n
									});
							}
							$(m).click(function (t) {
									var s = $(this);
									j.clickEngine(s, s.data("categoryIndex"), s.data("engineIndex"));
								});
							var o = $("<label></label>", {
									"for" : n,
									text : q.engineName
								});
							i.storeEngineData(m, l, r);
							p.append(m).append(o);
							k.append(p);
							if (c.current.getEngineIndex() === r) {
								o.addClass("checked");
								m.attr("checked", "checked");
							}
						});
					k.show();
				},
				drawLogo : function (j, l) {
					var i = h[j].engines[l];
					var k = f.host().find(".search-logo");
					k.css("background-image", "url('" + i.logo.img + "')");
					k.attr("href", i.logo.url);
					c.cookie.setSearchingServiceCookie(j, l);
				},
				changeButtonText : function (j, k) {
					var i = h[j].engines[k];
					$("#search-go").val(i.buttonText);
				}
			},
			events : {
				clickCategory : function (r, q) {
					var j = $("#search-form");
					var p = j.find(".search-tabs");
					var m = j.find(".search-engines");
					var l = c.drawProcess;
					var k = h[q];
					var n = 0;
					var o = null;
					c.current.privateCategoryIndex = q;
					n = c.cookie.getSearchingServiceCookie(q);
					c.current.privateEngineIndex = n;
					o = k.engines[n];
					p.find("a.cur").removeClass("cur");
					p.find("a:eq(" + q + ")").addClass("cur");
					l.drawEngines(q);
					l.drawLogo(q, n);
					l.changeButtonText(q, n);
					c.cookie.setSearchingServiceCookie(q, n);
					f.statistic({
							n : k.title + "-" + o.engineName,
							v : "",
							m : "b"
						});
					var i = $("#search-key");
					i.focus();
					if (i.val().length > 0) {
						i.blur();
					}
				},
				clickEngine : function (t, u, p) {
					var l = $("#search-form");
					var o = l.find(".search-engines");
					var r = c.current;
					var m = r.getEngineIndex();
					var n = o.find("span:eq(" + m + ")");
					var q = o.find("span:eq(" + p + ")");
					var k = c.drawProcess;
					var j = h[u];
					var s = j.engines[p];
					n.find("label").removeClass("checked");
					q.find("label").addClass("checked");
					t.attr("checked", "checked");
					k.drawLogo(u, p);
					k.changeButtonText(u, p);
					c.cookie.setSearchingServiceCookie(u, p);
					f.statistic({
							n : s.engineName + "-" + j.title,
							v : "",
							m : "b"
						});
					var i = $("#search-key");
					i.focus();
					if (i.val().length > 0) {
						i.blur();
					}
				}
			},
			store : {
				storeCategoryData : function (j, i) {
					j.data("categoryIndex", i);
				},
				storeEngineData : function (j, i, k) {
					j.data("categoryIndex", i);
					j.data("engineIndex", k);
				}
			},
			other : {
				init : function () {
					var k = c.cookie.getSearchingServiceCookie(0);
					var j = c.drawProcess;
					var i = c.other;
					j.drawCategories();
					j.drawEngines(0);
					j.drawLogo(0, k);
					j.changeButtonText(0, k);
					i.bindSubmit();
					i.miscellaneous();
				},
				bindSubmit : function () {
					$("#search-form").submit(function (j) {
							var t = $.trim($("#search-key").val());
							var p = c.current;
							var s = p.getCategoryIndex();
							var l = h[s];
							var o = p.getEngineIndex();
							var q = l.engines[o];
							var u = q.submitData;
							var m = u.params;
							var r;
							var i = $("#submitParamsLayer");
							var k = $("#search-form");
							e();
							if (i.length > 0) {
								i.remove();
							}
							i = $("<div></div>", {
									id : "submitParamsLayer"
								});
							k.attr("action", encodeURI(u.action.replace("{KEY}", t)));
							k.attr("accept-charset", u.charset);
							document.charset = u.charset;
							for (var n in m) {
								if (m[n] != "{KEY}") {
									r = $("<input name='" + n + "' type='hidden' value= '" + m[n] + "' />");
								} else {
									r = $("<input name='" + n + "' type='hidden' value= '" + t + "' />");
								}
								i.append(r);
							}
							k.append(i);
							f.statistic({
									n : "点击搜索按钮",
									v : l.title + " - " + q.engineName + " - " + t,
									m : "b"
								});
							j.stopPropagation();
						});
				},
				miscellaneous : function () {
					$(document).ready(function () {
							var i = $("#search-key");
							i.val("");
							i.focus();
						});
					if ($.browser.msie) {
						$("#search-key-wrapper")[0].onselectstart = function () {
							return false;
						};
						$("#search-key").focus(function () {
								$("#search-key-wrapper")[0].onselectstart = null;
							});
						$("#search-key").blur(function () {
								$("#search-key-wrapper")[0].onselectstart = function () {
									return false;
								};
							});
					} else {
						if ($.browser.webkit) {
							$("#search-key-wrapper")[0].onselectstart = function () {
								return false;
							};
						}
					}
				}
			}
		};
		c.other.init();
		window.google = {
		};
		window.google.ac = {
		};
		window.google.ac.h = function (j) {
			var i = j[1];
			if (!i || i.length <= 0) {
				e();
				return;
			}
			g(i);
		};
		function e() {
			var i = $("#reminderLayer");
			if (i[0]) {
				i.remove();
			}
			f.notify("module-search-reminder-layer-removed");
		}
		function b() {
			var k = $("<div id='reminderLayer' class='reminder-layer'></div>");
			f.host().append(k);
			var l = $("#search-key-wrapper").offset();
			var j,
			i;
			j = l.top + $("#search-key-wrapper")[0].clientHeight;
			i = 92;
			k.css("top", j + "px");
			k.css("left", i + "px");
			return k;
		}
		function g(k) {
			e();
			var n = b();
			for (var m = 0; m < k.length; m++) {
				var l = k[m][0];
				var j = $("<a href='#'>" + l + "</a>");
				n.append(j);
			}
			$("#reminderLayer a").click(function (o) {
					o.preventDefault();
					o.stopPropagation();
					var i = $.trim($(this).text());
					$("#search-key").val(i);
					$("#search-form").submit();
				});
			$("#reminderLayer a").hover(function () {
					$("#reminderLayer a.selected").removeClass("selected");
					$(this).addClass("selected");
					$("#search-key").val($(this).text());
					f.notify("reminder-layer-selected");
				}, function () {
					$(this).removeClass("selected");
				});
			f.notify("module-search-reminder-layer-created");
		}
		$("#search-key").keyup(function (i) {
				if ($.inArray(i.which, [37, 38, 39, 40]) >= 0) {
					return true;
				}
				f.request({
						type : "script",
						url : "http://www.google.cn/complete/search?hl=zh-CN&q=" + encodeURI(this.value)
					}, function () {
					});
			});
		$(document).click(function () {
				e();
			});
		$(document).keydown(function (i) {
				if (!document.getElementById("reminderLayer")) {
					return;
				}
				if ($.inArray(i.which, [37, 39]) >= 0) {
					f.notify("switch-keyboard-focus");
					return false;
				} else {
					if ($.inArray(i.which, [38, 40]) >= 0) {
						var j = (i.which == 38) ? "up" : "down";
						if (d()) {
							f.notify("keyboard-control-suggestion-panel", j);
						} else {
							a(j);
						}
						return false;
					} else {
						if (i.which == 13) {
							if (d()) {
								f.notify("keyboard-submit");
							}
						}
					}
				}
			});
		function d() {
			var i = $("#suggestion-panel");
			if (i.length > 0 && i.data("focus")) {
				return true;
			} else {
				return false;
			}
		}
		function a(m) {
			var j = $("a", $("#reminderLayer"));
			var l = $("a.selected", $("#reminderLayer"));
			var i = j.index(l);
			var k = j.length;
			if (m == "up") {
				i--;
			} else {
				if (m == "down") {
					i++;
				}
			}
			if (i < 0) {
				i = k - 1;
			} else {
				if (i > k - 1) {
					i = 0;
				}
			}
			l.removeClass("selected");
			$(j[i]).addClass("selected");
			$("#search-key").val($(j[i]).text());
		}
		f.listen("hook-search", function (n) {
				var q = ["img", "txt", "links", "login"];
				var k = {
					img : {
						layout : function (s, r, t) {
							var u = 2;
							$(s).each(function (z, y) {
									var v = $("<span></span>", {
											"class" : "image-link-background"
										});
									var B = $("<a></a>", {
											"class" : "image",
											title : y.txt,
											href : y.img_url,
											target : "_blank",
											"data-name" : t + " -> " + y.txt
										});
									var x = $("<span></span>");
									var w = new Image();
									w.className = y.size + "-image";
									if ($.browser.msie && $.browser.version == "6.0" && y.size == "big") {
										w.onreadystatechange = function () {
											if (w.readyState == "loaded" || w.readyState == "complete") {
												var C = w.width / w.height;
												if (w.width > 169 || w.height > 64) {
													if (C >= 169 / 64) {
														w.width = 169;
														w.height = w.width / C;
													} else {
														w.height = 64;
														w.width = w.height * C;
													}
												}
												B.append(x.append(w));
											}
										};
										w.src = y.img_src;
									} else {
										w.src = y.img_src;
										B.append(x.append(w));
									}
									B.hover(function () {
											$(this).find("img").fadeTo("fast", 0.7);
										}, function () {
											$(this).find("img").fadeTo("fast", 1);
										});
									v.append(B);
									if (y.size == "big") {
										v.addClass("first");
										r.append(v);
										if (y.remark) {
											var A = j(y.remark);
											r.append(A);
										}
										r.append(i());
									} else {
										if (u > 0) {
											r.append(v);
										}
										u--;
										if (u <= 0) {
											r.append(i());
											u = 2;
										} else {
											v.addClass("first");
										}
									}
									o.keyMouseNavigation(v, y.img_url);
								});
						}
					},
					txt : {
						layout : function (s, r, t) {
							$(s).each(function (v, u) {
									var w = $("<a></a>", {
											"class" : "text",
											text : u.txt,
											target : "_blank",
											href : u.txt_url,
											"data-name" : t + " -> " + u.txt
										});
									if (u.remark) {
										r.append(w).append(j(u.remark)).append(i());
									} else {
										r.append(w).append(i());
									}
									o.keyMouseNavigation(w, u.txt_url);
								});
						}
					},
					links : {
						layout : function (u, r, v) {
							var t = $("<span></span>", {
									"class" : "links clearfix"
								});
							var s = (u.items) ? u.items : u;
							$(s).each(function (x, w) {
									var y = $("<a></a>", {
											text : w.txt,
											target : "_blank",
											href : w.url,
											"data-name" : v + " -> " + w.txt
										});
									t.append(y);
									o.keyMouseNavigation(y, w.url);
								});
							r.append(t);
							if (u.remark) {
								r.append(j(u.remark));
							}
							r.append(i());
						}
					},
					email : {
						layout : function (x, s, C) {
							var D = $("<span></span>", {
									"class" : "email clearfix"
								});
							var t = $("<form></form>", {
									action : x.login.url,
									target : "_blank",
									method : "post",
									submit : function () {
										$("#suggestion-email-params-layer").remove();
										var G = $("<div></div>", {
												id : "suggestion-email-params-layer",
												css : {
													display : "none"
												}
											});
										for (var J in x.login.params) {
											var I = x.login.params[J];
											if (I.indexOf("{USR}") >= 0) {
												I = I.replace("{USR}", y.val());
											} else {
												if (I.indexOf("{PWD}") >= 0) {
													I = I.replace("{PWD}", v.val());
												}
											}
											var H = $("<input />", {
													type : "hidden",
													name : J,
													value : I
												});
											G.append(H);
										}
										$(this).append(G);
										f.statistic({
												m : "搜索建议词",
												n : C + " -> 邮箱登录",
												u : ""
											});
									}
								});
							var F = $("<div></div>");
							var z = $("<span>账号：</span>");
							var y = $("<input />", {
									"class" : "suggestion-input email-input",
									keydown : function (G) {
										if (G.which == 13) {
											u.click();
											return false;
										}
									}
								});
							var B = $("<span></span>", {
									"class" : "email-domain",
									text : "@" + x.domain
								});
							F.append(z).append(y).append(B);
							var E = $("<div></div>");
							var w = $("<span>密码：</span>");
							var v = $("<input />", {
									"class" : "suggestion-input email-input",
									type : "password",
									keydown : function (G) {
										if (G.which == 13) {
											u.click();
											return false;
										}
									}
								});
							E.append(w).append(v);
							var A = $("<div></div>", {
									"class" : "clearfix"
								});
							var u = $("<input />", {
									"class" : "suggestion-button email-submit",
									type : "button",
									href : x["submit-url"],
									click : function () {
										t.submit();
										e();
									}
								});
							u.val("登录");
							var r = $("<a></a>", {
									"class" : "suggestion-link email-register",
									text : "注册",
									href : x["register-url"],
									"data-name" : C + " -> 邮箱注册",
									click : function () {
										window.open(x["register-url"]);
										setTimeout(e, 100);
									}
								});
							A.append(u).append(r);
							D.append(t);
							t.append(F).append(E).append(A);
							s.append(D);
							if (x.remark) {
								s.append(j(x.remark));
							}
							s.append(i());
							o.keyMouseNavigation(r, x["register-url"]);
						}
					}
				};
				function i() {
					return $("<span />", {
							"class" : "separator"
						});
				}
				function j(r) {
					return $("<span></span>", {
							"class" : "description",
							text : r,
							click : function () {
								return false;
							}
						});
				}
				function l(r) {
					this.list = (function () {
							var s = [];
							$(r).each(function (u, t) {
									$(t.words).each(function (w, v) {
											var x = {
												word : v.replace(/[\s,，]/g, ""),
												index : u,
												priority : (t.priority) ? parseInt(t.priority) : 999,
												title : t.title
											};
											s.push(x);
										});
								});
							s.sort(function (u, t) {
									return u.priority - t.priority;
								});
							return s;
						})();
					this.cachedItems = [];
					this.cachedIndex = 0;
				}
				l.prototype.match = function (s) {
					s = s.replace(/[\s,，]/g, "");
					var r = [];
					if (s.length <= 0) {
						return[];
					}
					return $.grep(this.list, function (u, t) {
							if (s.indexOf(u.word) == 0 && $.inArray(u.index, r) < 0) {
								r.push(u.index);
								return true;
							} else {
								return false;
							}
						});
				};
				l.prototype.deselect = function () {
					$(this.cachedItems).each(function (s, r) {
							if ($.isArray(r)) {
								if (r[0].hasClass("selected")) {
									$(r).each(function (t, u) {
											u.removeClass("selected");
										});
								}
							} else {
								if (r.hasClass("selected")) {
									r.removeClass("selected");
								}
							}
						});
				};
				l.prototype.select = function (s) {
					var t = $("#suggestion-panel");
					var u = $("#reminderLayer");
					if (!t.data("focus")) {
						t.data("focus", true);
						u.find("a.selected").removeClass("selected");
					}
					this.deselect();
					if (typeof(s) == "number" && s >= 0) {
						this.cachedIndex = s;
					}
					var r = this.cachedItems[this.cachedIndex];
					if ($.isArray(r)) {
						if (!r[0].hasClass("selected")) {
							$(r).each(function (v, w) {
									w.addClass("selected");
								});
						}
					} else {
						if (!r.hasClass("selected")) {
							r.addClass("selected");
						}
					}
				};
				l.prototype.selectNext = function () {
					this.cachedIndex = (this.cachedIndex == this.cachedItems.length - 1) ? 0 : this.cachedIndex + 1;
					this.select();
				};
				l.prototype.selectPrevious = function () {
					this.cachedIndex = (this.cachedIndex == 0) ? this.cachedItems.length - 1 : this.cachedIndex - 1;
					this.select();
				};
				l.prototype.isSelected = function () {
					var r = false;
					$(this.cachedItems).each(function (s, t) {
							if ($.isArray(t)) {
								r = t[0].hasClass("selected");
							} else {
								r = t.hasClass("selected");
							}
							if (r) {
								return false;
							}
						});
					return r;
				};
				l.prototype.go = function () {
					var s = this.cachedItems[this.cachedIndex];
					var r;
					if ($.isArray(s)) {
						r = s[0];
					} else {
						r = s;
					}
					if (!r.data("url")) {
						return;
					}
					window.open(r.data("url"));
				};
				l.prototype.reset = function (r) {
					this.cachedIndex = 0;
					if (r) {
						this.cachedItems.length = 0;
					}
				};
				l.prototype.statistic = function () {
					var s = this.cachedItems[this.cachedIndex];
					var r;
					var u = {
					};
					if ($.isArray(s)) {
						r = s[0];
					} else {
						r = s;
					}
					if (r.find("img").length > 0) {
						var t = r.find("a");
						u.n = t.attr("data-name");
						u.u = t.attr("href");
					} else {
						if (r[0].tagName.toLowerCase() == "a") {
							u.n = r.attr("data-name");
							u.u = r.attr("href");
						} else {
							return;
						}
					}
					u.m = "搜索建议词";
					f.statistic(u);
				};
				l.prototype.keyMouseNavigation = function (u, t) {
					var r = this;
					r.cachedItems.push(u);
					if (t) {
						u.data("url", t);
					}
					var s = r.cachedItems.length - 1;
					u.hover(function () {
							r.select(s);
						}, function () {
							r.deselect();
						});
				};
				function m(s) {
					o.reset(true);
					var t = s[0].title ? s[0].title : "i need title...";
					var r = (function () {
							if ($("#suggestion-panel").length > 0) {
								return $("#suggestion-panel");
							}
							var u = $("#search-key-wrapper").offset();
							var v = $("<div></div>", {
									id : "suggestion-panel",
									css : {
										top : u.top + $("#search-key-wrapper").height() + 1 + "px",
										left : 315 + "px"
									},
									click : function (w) {
										if (!o.isSelected()) {
											return false;
										}
									},
									"data-item" : "搜索建议词"
								});
							return v;
						})();
					(function (u) {
							u.append($("<div></div>", {
										text : t,
										"class" : "suggestion-title"
									}));
						})(r);
					(function (u) {
							for (var v = 0; v < s.length; v++) {
								var w = s[v];
								if (n[w.index].sequence) {
									q = n[w.index].sequence;
								}
								$(q).each(function (x, y) {
										var z = n[w.index][y];
										if (z) {
											k[y].layout(z, u, t);
										}
									});
							}
							u.find("span.separator:last").remove();
						})(r);
					$("#search-bar").append(r);
					$("#reminderLayer").find("a").addClass("shrink");
				}
				function p() {
					var r = $("#suggestion-panel");
					if (r.length > 0) {
						r.remove();
					}
				}
				var o = new l(n);
				f.listen("module-search-reminder-layer-created", function () {
						var r = o.match($("#search-key").val());
						if (r.length <= 0) {
							p();
							return;
						}
						m(r);
					});
				f.listen("module-search-reminder-layer-removed", function () {
						p();
					});
				f.listen("switch-keyboard-focus", function () {
						var s = $("#suggestion-panel");
						var t = $("#reminderLayer");
						var r = $("#search-key");
						if (!s.data("focus")) {
							s.data("focus", true);
							t.find("a.selected").removeClass("selected");
							o.reset();
							o.select();
						} else {
							s.removeData("focus");
							o.deselect();
							t.find("a:eq(0)").addClass("selected");
						}
					});
				f.listen("keyboard-control-suggestion-panel", function (r) {
						if (r == "up") {
							o.selectPrevious();
						} else {
							o.selectNext();
						}
					});
				f.listen("keyboard-submit", function () {
						$("#search-key").blur();
						o.go();
						e();
						o.statistic();
					});
				f.listen("reminder-layer-selected", function () {
						var r = $("#suggestion-panel");
						if (r.data("focus")) {
							o.deselect();
							r.removeData("focus");
						}
					});
			});
	});
Core.reg("search-scroll-keys", function (c) {
		var b = {
		};
		c.listen("start-hotkeys", function (d) {
				b = d;
				a();
			});
		c.listen("request-hotkeys", function () {
				c.notify("response-hotkeys", b);
			});
		function a() {
			var e = $("#search-scroll-keys");
			var d = [];
			e.empty();
			if (!b.hot) {
				return;
			}
			for (var g = 0; g < b.hot.length; g++) {
				var j = b.hot[g];
				if (j.type.indexOf("s") == -1 || !j.n) {
					continue;
				}
				var f = "";
				if (j.l) {
					d.push({
							title : j.n,
							link : j.l
						});
				} else {
					for (var h in b.engineList) {
						if (j.type.indexOf(h.toString()) != -1) {
							f = b.engineList[h];
							break;
						}
					}
					d.push({
							title : j.n,
							link : f + encodeURIComponent(j.n)
						});
				}
			}
			e.links(d);
		}
	});
Core.reg("widget-center", function (j) {
		var p = {
			none : {
				title : "",
				hidden : true
			},
			websites : {
				title : "网址导航",
				url : "/html/widget_urls.htm",
				initialHeight : 680
			},
			news : {
				title : "今日新闻",
				url : "/html/widget_news.htm",
				initialHeight : 592
			},
			images : {
				title : "美图秀",
				url : "/html/widget_images.htm",
				initialHeight : 806
			},
			games : {
				title : "休闲游戏",
				url : "/html/widget_games.htm",
				initialHeight : 572
			},
			books : {
				title : "热门书籍",
				url : "/html/widget_books.htm",
				initialHeight : 549
			},
			shopping : {
				title : "淘宝商城",
				url : "/html/widget_shopping.htm",
				initialHeight : 1226
			},
			tools : {
				title : "实用工具",
				url : "/html/widget_tools.htm",
				initialHeight : 835
			},
			music : {
				title : "音乐听听",
				url : "/html/widget_music.htm",
				css : {
					width : "580px",
					left : "380px"
				},
				initialHeight : 622
			}
		};
		var m = ["none", "websites", "news", "games", "books", "shopping", "tools", "images", "music"];
		var b = [0, 12, 99, 188, 276, 364, 452, 540, 628, 716];
		var t = 500;
		var u = $("#widget-layer");
		var q = $("#widget-center-tabs");
		var f,
		n,
		l;
		var i = $("#widget-center-current-layer");
		var g = i.find(".inner");
		var o = 0;
		var c = {
			"horizontal-expand-time" : 300,
			"vertical-expand-time" : 0,
			"minimum-expand-width" : 89,
			"minimum-expand-height" : 15,
			"initial-left" : 0,
			"refresh-times" : 20,
			damping : 600
		};
		if (mxapi.version != "other") {
			m = ["none", "websites", "last", "news", "games", "books", "shopping", "tools", "images", "music"];
			p.last = {
				title : "上次访问",
				url : "/html/widget_last.htm",
				css : {
					width : "362px"
				}
			};
		}
		j.listen("hook-widget-center", function (w) {
				if (w.resources) {
					for (var v in w.resources) {
						p[v] = w.resources[v];
					}
				}
				if (w.order) {
					m = w.order;
				}
			});
		function r() {
			j.statistic({
					n : "c0",
					v : ($.cookieAdvanced("ch.new", "widget-center-mode") || 0),
					m : "c"
				});
		}
		j.listen("start-widget-center", function (v) {
				$("#widget-center-btn-more").click(function (w) {
						j.host().toggleClass("narrow");
						$.cookieAdvanced("ch.new", "widget-center-mode", j.host().hasClass("narrow") ? "1" : "0");
						r();
						w.stopPropagation();
						return false;
					});
				if ($.cookieAdvanced("ch.new", "widget-center-mode") == 1) {
					j.host().toggleClass("narrow");
				}
				r();
				$.each(m, function (x, y) {
						var w = p[y];
						$('<a style="left: ' + b[x] + 'px" href="#"></a>').text(w.title).attr("id", "btn-widget-" + y).appendTo(q);
						$('<div class="panel module"></div>').appendTo(u);
					});
				$(q.find("a")[0]).addClass("first-tab");
				f = u.find(".panel");
				l = $(f[0]);
				q.tabs("#widget-layer .panel", {
						effect : "folderForWidget",
						onBeforeClick : function (x, w) {
							s(this, w);
						}
					});
				l.empty().css("display", "none");
				n = q.data("tabs");
				i.click(function (x) {
						var w = n.getIndex();
						if (m[w] === "music") {
							j.notify("w_c_close-music");
						} else {
							k();
						}
						x.preventDefault();
					});
				q.find("a").click(function () {
						j.notify("show-widget-news-panel", {
								index : 0
							});
					});
			});
		j.listen("statistic-close", function (x) {
				var v = {
					music : {
						n : "p0",
						m : "p"
					},
					last : {
						n : "q1",
						m : "q"
					}
				};
				var w = v[x.name];
				j.statistic({
						n : w.n,
						v : x.flag,
						m : w.m
					});
			});
		j.listen("show-widget", function (v) {
				widgetIndex = $.inArray(v.id, m);
				n.click(widgetIndex);
				j.listen("request-news-currentTab", function () {
						j.notify("show-widget-news-panel", {
								index : v.index
							});
					});
				j.notify("show-widget-news-panel", {
						index : v.index
					});
				j.notify("setScroll");
			});
		j.listen("setScroll", function () {
				$("html, body").animate({
						scrollTop : j.host().position().top - 5
					}, "slow");
			});
		j.listen("setScrollInWidgetImages", function (w) {
				if (document.documentElement.scrollTop >= 290) {
					return;
				}
				setInterval(function () {
					}, 100);
				var v = j.host().position().top + 112;
				$("html, body").animate({
						scrollTop : v - 10
					}, {
						duration : 500,
						queue : true
					});
				$("html, body").animate({
						scrollTop : v
					}, {
						duration : c.damping,
						queue : true,
						complete : function () {
							w.unbind("mousewheel");
							w.data("mouseWheelLock", "unlock");
						}
					});
			});
		j.listen("hide-widget", function (v) {
				if (v) {
					v.flag ? k(v.flag) : k();
					return;
				}
				k();
			});
		function s(B, x) {
			if (x != o) {
				j.notify("widget-" + m[o] + "-collapse");
			}
			var z = $("#widget-shadow");
			z.hide();
			var y = m[x];
			var v = p[y];
			var w = $(f[x]);
			if (x > 0) {
				var A = $("#widget-center-tabs a:eq(" + x + ")");
				i.show().attr("class", y + "-current").css("left", (A.position().left - 12) + "px").css("top", (A.position().top - 3) + "px");
				g.text(v.title);
			}
			e(B, x, function () {
					if (w.attr("status") != "completed") {
						if (v.url) {
							w.empty();
							var C = $('<iframe frameborder="0" marginheight="0" marginwidth="0" scrolling="no" width="100%"></iframe>').attr("src", v.url + "?v=" + j.getCacheVersion()).attr("id", "widget-frame-" + y);
							if (v.initialHeight != undefined) {
								C.attr("height", v.initialHeight + "px");
							}
							C.appendTo(w);
							C.load(function () {
									w.attr("status", "completed");
								});
							var D = $("<span></span>", {
									"class" : "widget-top-border",
									css : {
										left : function () {
											var E = 0;
											if (v.css && v.css.left) {
												E = parseInt(v.css.left.substring(0, v.css.left.length - 2));
											}
											return A.position().left + 76 - E + "px";
										}
									}
								});
							D.appendTo(w);
						}
					}
					j.notify("widget-" + m[x] + "-expand");
					o = x;
					if (v.css) {
						if (v.css.width) {
							u.css("width", v.css.width);
						} else {
							u.css("width", "960px");
						}
						if (v.css.left) {
							u.css("left", v.css.left);
						} else {
							u.css("left", "0px");
						}
					} else {
						u.css("width", "960px");
						u.css("left", "0px");
					}
				});
		}
		var h = null;
		function d(y, C, E) {
			var D = $("#animation-div");
			var v = $("#widget-center-current-layer");
			var w = y.getPanes().eq(C);
			var B = $("#widget-center");
			var z = p[m[C]];
			var x = {
				left : (function (F) {
						if (F.css && F.css.left) {
							return F.css.left;
						} else {
							return "0px";
						}
					})(z),
				height : (function (F) {
						if (F.initialHeight) {
							return F.initialHeight + "px";
						} else {
							if (m[C] == "last") {
								return 43 + mxapi.history.lastOpenList().length * 34 + "px";
							} else {
								return "100px";
							}
						}
					})(z),
				width : (function (F) {
						if (F.css && F.css.width) {
							return F.css.width;
						} else {
							return "958px";
						}
					})(z)
			};
			function A() {
				D.data("currentLoadingIndex", C);
				D.animate({
						left : x.left,
						width : x.width
					}, c["horizontal-expand-time"], function () {
						D.animate({
								height : x.height
							}, c["vertical-expand-time"], function () {
								var I = m[C];
								if (I == "last" && !w.attr("status")) {
									j.listen("position-shadow", function (J) {
											w.data("shadow", {
													height : J.height + 2 + "px",
													width : J.width + 2 + "px"
												});
											a(w, C);
										});
								} else {
									a(w, C);
								}
								D.append($("<span></span>", {
											"class" : "widget-waiting"
										}));
								E.call();
								var H = 0;
								function F() {
									if (H >= c["refresh-times"]) {
										G();
										return;
									}
									if (C != D.data("currentLoadingIndex")) {
										return;
									}
									if (!w.attr("status") && $.inArray(I, ["shopping", "music", "last", "images"]) < 0) {
										H++;
										setTimeout(F, 500);
										return;
									}
									D.empty();
									D.hide();
									w.show();
								}
								function G() {
									if (C != D.data("currentLoadingIndex")) {
										return;
									}
									var J = [];
									J.push("<div class='widget-overtime' style='height: " + x.height + "; line-height: " + x.height + ";'>");
									J.push("<span>页面加载超时，请单击<a href='#'>此处</a>刷新。</span>");
									J.push("</div>");
									D.html(J.join(""));
									setTimeout(function () {
											D.find("a").click(function (K) {
													K.preventDefault();
													H = 0;
													D.empty();
													D.append($("<span></span>", {
																"class" : "widget-waiting"
															}));
													E.call();
													F();
												}).focus(function () {
													$(this).blur();
												});
										}, 500);
								}
								F();
							});
					});
			}
			if (D.length <= 0) {
				D = $("<div></div>", {
						id : "animation-div",
						css : {
							left : v.position().left + c["initial-left"] + "px",
							top : v.position().top + 58 + "px",
							width : c["minimum-expand-width"] + "px",
							height : c["minimum-expand-height"] + "px",
							position : "absolute",
							border : "1px solid #A1ADB9",
							"background-color" : "white",
							display : "block"
						}
					});
				B.append(D);
				A();
			} else {
				D.css({
						width : c["minimum-expand-width"] + "px",
						left : v.position().left + c["initial-left"] + "px",
						height : c["minimum-expand-height"] + "px",
						display : "block"
					});
				setTimeout(A, 100);
			}
			h = {
				left : v.position().left + c["initial-left"] + "px"
			};
		}
		function e(y, v, x) {
			y.getPanes().hide();
			var w = $("#animation-div");
			w.empty();
			if (v == 0 && w.length <= 0) {
				return;
			}
			if (v == 0) {
				w.show();
				w.animate({
						height : c["minimum-expand-height"] + "px"
					}, c["vertical-expand-time"], function () {
						w.animate({
								width : c["minimum-expand-width"] + "px",
								left : h.left
							}, c["horizontal-expand-time"], function () {
								w.hide();
								i.hide().css("left", "0");
								g.text("");
								j.notify("widget-" + m[o] + "-collapse");
							});
					});
			} else {
				d(y, v, x);
			}
		}
		function k(x) {
			var w = n.getIndex();
			x = x || 0;
			if (x) {
				var v = $(f[w]);
				v.empty().attr("status", "");
			}
			if ($.inArray(m[w], ["music", "last"]) !== -1) {
				j.notify("statistic-close", {
						name : m[w],
						flag : x
					});
			}
			n.click(0);
			l.css("display", "none");
		}
		function a(v, A) {
			var z = $("#widget-shadow");
			function D(E, F) {
				return parseInt(E.substring(0, E.length - 2)) + F + "px";
			}
			var B = v.data("shadow");
			var y = p[m[A]];
			var w,
			C,
			x;
			if (B) {
				w = B.width;
			} else {
				w = (!y.css) ? "960px" : (!y.css.width ? "960px" : y.css.width);
			}
			if (B) {
				C = B.height;
			} else {
				C = y.initialHeight + 2 + "px";
			}
			var x = (!y.css) ? "2px" : ((!y.css.left) ? "2px" : D(y.css.left, 2));
			if (A > 0) {
				z.css({
						left : x,
						width : w,
						height : C,
						display : "block"
					});
			} else {
				z.css("display", "none");
			}
		}
	});
Core.reg("favorites", function (q) {
		var j = false;
		var b = false;
		var A = -1;
		var d = null;
		var o = {
		};
		var u = -1;
		var m = false;
		var s = '<ul id="dialog-favorites">    <li>标题</li>    <li>        <input type="text" id="txt-title" />    </li>    <li>网址</li>    <li>        <input type="text" id="txt-url" value="http://" />    </li>    <li>        <p>            注意:添加网址只会修改起始页内容，不会改变浏览器内其他应用和已有收藏夹的内容        </p>    </li></ul>';
		var C = '<ul id="dialog-favorites">    <li>标题</li>    <li>        <input type="text" id="txt-title" />    </li>    <li>网址</li>    <li>        <input type="text" id="txt-url" />    </li>    <li>        <p>            注意:编辑网址只会修改起始页内容，不会改变浏览器内其他应用和已有收藏夹的内容        </p>    </li></ul>';
		var p = '<ul id="dialog-favorites">    <li>你确定要删除网址?</li>    <li>        <p>            注意:删除网址只会修改起始页内容，不会改变浏览器内其他应用和已有收藏夹的内容        </p>    </li></ul>';
		var x = '<ul id="dialog-favorites">    <li>你确定要恢复收藏?</li>    <li>        <p>            恢复经常访问收藏为默认数据，可能会丢失自定义内容。        </p>    </li></ul>';
		function n(F, E) {
			var D = {
				dialogTitle : "",
				initWidth : 340,
				content : F,
				buttonText : {
					ok : "确定",
					cancel : "取消"
				}
			};
			if (!($.browser.msie && $.browser.version == "6.0")) {
				D.fixed = "true";
			}
			D = $.extend({
				}, D, E);
			$.fn.jmodal(D);
		}
		function h(E) {
			var D = $("<li></li>", {
					"class" : "clearfix"
				});
			if (E.children().length == 1) {
				D.addClass("even");
			}
			return D.clone();
		}
		function w(E) {
			var D = $("#fav-list");
			if (E.tagName === "SPAN") {
				E = E.parentNode;
			}
			return parseInt($(E).attr("index"));
		}
		function a() {
			var E = $("#fav-list");
			var F = mxapi.favorites.list();
			E.empty();
			$(F).each(function (G, I) {
					var H = null;
					if (E.children().length <= 0 || E.find("li:last > a").length >= 9) {
						H = h(E);
						E.append(H);
					} else {
						H = E.children().last();
					}
					var J = $("<a></a>", {
							target : "_blank",
							href : I.url,
							html : "<span class='title'>" + I.title + "</span><span title='编辑'></span><span title='删除'></span>",
							index : G
						});
					J.find("span:eq(1)").hover(function () {
							$(this).addClass("edit-hover");
						}, function () {
							$(this).removeClass("edit-hover");
						});
					J.find("span:eq(2)").hover(function () {
							$(this).addClass("remove-hover");
						}, function () {
							$(this).removeClass("remove-hover");
						});
					H.append(J);
					if (j) {
						r(J);
					}
				});
			while (E.children().length < 3) {
				var D = h(E);
				E.append(D);
			}
			q.host().append(E);
			e($("span[class='title']", $("#fav-list")), 175);
		}
		function e(D, E) {
			if ($.browser.msie && $.browser.version == "6.0") {
				D.each(function () {
						if ($(this).width() > E) {
							$(this).css("width", E + "px");
						}
					});
			}
		}
		function r(D) {
			D.find("span:eq(1)").addClass("edit-btn").bind("click", function (G) {
					G.preventDefault();
					var F = w(this);
					var H = $(this).prev().html();
					var E = $(this).parent().attr("href");
					n(C, {
							dialogTitle : "编辑网址",
							data : {
								title : H,
								url : E
							},
							okHandler : function (K, J) {
								var L = $("#txt-title").val();
								var I = $("#txt-url").val();
								if (L == "" || I == "" || I == "http://") {
									alert("标题或者网址不能为空。");
									return;
								}
								I = g(I);
								l(F, L, I);
								J.close();
							},
							loadCompleteHandler : function (I) {
								$("#txt-title").val(I.title);
								$("#txt-url").val(I.url);
							}
						});
				});
			D.find("span:eq(2)").addClass("remove-btn").bind("click", function (F) {
					F.preventDefault();
					var E = w(this);
					n(p, {
							dialogTitle : "删除网址",
							okHandler : function (H, G) {
								B(E);
								G.close();
							}
						});
				});
			D.bind("mouseenter", function () {
					$(this).addClass("hover");
				}).bind("mouseleave", function () {
					$(this).removeClass("hover");
				});
			D.addClass("drag");
			D.click(function (E) {
					E.preventDefault();
				});
			if ($("#fav-list").find("a").length >= 27) {
				$("#fav-add").hide();
			} else {
				$("#fav-add").show();
			}
		}
		function c(D) {
			var F = q.host().parent().parent().offset();
			var E = $("<div></div>", {
					id : "fav-waiting",
					html : "<p>正在加载，请稍后...</p>",
					css : {
						position : "absolute",
						top : (F.top + 28) + "px",
						left : (F.left + 1) + "px",
						"text-align" : "center",
						"line-height" : "252px",
						width : q.host().width() + "px",
						background : "white url('/images/loading.gif') no-repeat center 40% scroll",
						"z-index" : "99"
					}
				});
			$(document.body).append(E);
			setTimeout(function () {
					D();
					E.remove();
				}, 10);
		}
		function f() {
			var F = $(".more", $("#main"));
			var G = $("<div onselectstart='return false;'></div>");
			if (mxapi.version == "1.x") {
				F.append(G);
				return;
			}
			var D = $("<a></a>", {
					id : "fav-start-edit",
					href : "#",
					html : "编辑链接",
					"data-name" : "btn-edit",
					click : function (L) {
						L.preventDefault();
						j = true;
						G.children().show();
						$(this).hide();
						var K = $("#fav-list");
						r(K.find("a"));
					}
				});
			var I = $("<span></span>", {
					id : "fav-info",
					html : "(拖动条目可以改变位置)",
					css : {
						display : "none"
					}
				});
			var H = $("<a></a>", {
					id : "fav-reset",
					href : "#",
					html : "恢复默认",
					"data-name" : "btn-reset",
					css : {
						display : "none"
					},
					click : function (K) {
						n(x, {
								dialogTitle : "恢复默认收藏",
								okHandler : function (M, L) {
									L.close();
									z();
								}
							});
						K.preventDefault();
					}
				});
			var J = $("<a></a>", {
					id : "fav-add",
					href : "#",
					html : "添加网址",
					"data-name" : "btn-add",
					css : {
						display : "none"
					},
					click : function (K) {
						n(s, {
								dialogTitle : "添加网址",
								okHandler : function (N, M) {
									var O = $("#txt-title").val();
									var L = $("#txt-url").val();
									if (O == "" || L == "" || L == "http://") {
										alert("标题或者网址不能为空。");
										return;
									}
									L = g(L);
									v(O, L);
									M.close();
								},
								loadCompleteHandler : function (L) {
									$("#txt-title").focus();
								}
							});
						K.preventDefault();
					}
				});
			var E = $("<a></a>", {
					id : "fav-complete-edit",
					href : "#",
					html : "完成编辑",
					"data-name" : "btn-end",
					css : {
						display : "none"
					},
					click : function (M) {
						var L = $("#fav-list");
						var K = L.find("a");
						j = false;
						G.children().hide();
						D.show();
						K.unbind("mouseenter").unbind("mouseleave");
						K.find("span:eq(1)").removeClass("edit-btn").unbind("click");
						K.find("span:eq(2)").removeClass("remove-btn").unbind("click");
						K.unbind("click").removeClass("drag");
						M.preventDefault();
					}
				});
			G.append(D).append(E).append(J).append(H).append(I);
			F.append(G);
		}
		function z() {
			c(function () {
					mxapi.favorites.reset();
					a();
				});
		}
		function v(F, D) {
			mxapi.favorites.add(F, D);
			var E = $("#fav-list");
			E.children().each(function (I, J) {
					var G = $(J);
					var K = null;
					var H = $("#fav-list").find("a").length;
					if (G.children().length < 9) {
						var K = $("<a></a>", {
								target : "_blank",
								href : D,
								html : "<span class='title'>" + F + "</span><span title='编辑'></span><span title='删除'></span>",
								index : H
							});
						K.find("span:eq(1)").hover(function () {
								$(this).addClass("edit-hover");
							}, function () {
								$(this).removeClass("edit-hover");
							});
						K.find("span:eq(2)").hover(function () {
								$(this).addClass("remove-hover");
							}, function () {
								$(this).removeClass("remove-hover");
							});
						G.append(K);
						r(K);
						e(K.find("span[class='title']"), 175);
						return false;
					}
				});
		}
		function B(E) {
			var F = $("#fav-list");
			var D = F.children().eq(Math.floor(E / 9));
			var G = D.children().eq(E % 9);
			var J = false;
			var I = null;
			var H = null;
			F.find("a").each(function (K, L) {
					H = $(L);
					if (H[0] === G[0]) {
						J = true;
					}
					if (J) {
						I = parseInt(H.attr("index"));
						H.attr("index", (I - 1));
					}
				});
			mxapi.favorites.remove(E);
			G.remove();
			while (D.next().is("li") && D.next().children().length > 0) {
				D.append(D.next().children().eq(0));
				D = D.next();
			}
			if ($("#fav-add").css("display") === "none") {
				$("#fav-add").show();
			}
		}
		function l(G, K, F) {
			var E = Math.floor(G / 9);
			var I = G % 9;
			var H = $("#fav-list");
			var J = H.find("li:eq(" + E + ") a:eq(" + I + ")");
			var D = J.find("span[class='title']");
			J.attr("href", F);
			D.html(K);
			e(D, 175);
			mxapi.favorites.edit(G, K, F);
		}
		function g(D) {
			if (D.match(/^http:\/\//) == null) {
				D = "http://" + D;
			}
			return D;
		}
		q.listen("click-main-tab", function (F) {
				if (F.index != 1) {
					return;
				}
				var E = $("<ul></ul>", {
						id : "fav-list",
						"class" : "links-block"
					});
				q.host().append(E);
				if (mxapi.favorites.isObsolete()) {
					var D = $("<li></li>", {
							"class" : "obsolete",
							html : "<p>您当前使用的傲游浏览器版本无法编辑经常访问网站，<a href='http://www.maxthon.cn/download.htm' target='_blank'>请点击下载最新版</a>。</p>"
						});
					E.append(D);
				} else {
					f();
					c(function () {
							a();
						});
				}
				E.hover(function (G) {
						if (j && !m) {
							$(document.body).bind("mousedown", t);
							$(document.body).bind("mousemove", y);
							$(document.body).bind("mouseup", k);
							m = true;
						}
					}, function (G) {
						if (j && !b && m) {
							$(document.body).unbind("mousedown", t);
							$(document.body).unbind("mousemove", y);
							$(document.body).unbind("mouseup", k);
							m = false;
						}
					});
			});
		function t(E) {
			var F = E.target;
			var D = $("#fav-list");
			E.preventDefault();
			if (D.length <= 0) {
				return;
			}
			if (j && $.contains(D[0], F) && (F.tagName.toLowerCase() == "a" || (F.parentNode.tagName.toLowerCase() == "a" && $(F).hasClass("title")))) {
				b = true;
			}
			if (!b) {
				return;
			}
			if (F.tagName.toLowerCase() === "span") {
				d = $(F.parentNode);
			} else {
				d = $(F);
			}
			d.addClass("drag-selected");
			d.css({
					left : "0",
					top : "0"
				});
			o = {
				left : E.pageX,
				top : E.pageY
			};
			u = w(d);
		}
		function y(D) {
			D.preventDefault();
			if (b) {
				d.css({
						left : D.pageX - o.left,
						top : D.pageY - o.top
					});
				i({
						left : D.pageX,
						top : D.pageY
					});
			}
		}
		function k(D) {
			D.preventDefault();
			if (!b) {
				return;
			}
			if (A > -1 && A != u) {
				mxapi.favorites.exchangeOrder(u, A);
				setTimeout(a, 10);
			}
			d.removeClass("drag-selected");
			$("#fav-list").find("a").removeClass("drag-covered");
			b = false;
			A = -1;
			o = {
			};
		}
		function i(D) {
			var E = 0;
			var J = 0;
			var G = 239;
			var I = 26;
			var H = 5;
			var F = $("#fav-list").offset();
			D.left -= F.left;
			D.top -= F.top;
			for (; E < 3; E++) {
				if (D.left >= (E * G) && D.left < ((E + 1) * G)) {
					break;
				}
			}
			for (; J < 9; J++) {
				if (D.top >= (J * I + (Math.floor(J / 3) * H)) && D.top < ((J + 1) * I + (Math.floor(J / 3) * H))) {
					break;
				}
			}
			if (E >= 3 || J >= 9) {
				A = -1;
				return;
			}
			A = E + (J * 3);
			var K = $("#fav-list").find("a:eq(" + A.toString() + ")");
			$("#fav-list").find("a").removeClass("drag-covered");
			if (K.length > 0) {
				if (A != u) {
					K.addClass("drag-covered");
				} else {
					A = -1;
				}
			} else {
				A = -1;
			}
		}
	});
//Core.reg("news", function (i) {
//    var s = {
//        chief : {
//            title : "要闻"
//        },
//        ent : {
//            title : "娱乐"
//        },
//        social : {
//            title : "社会"
//        },
//        cars : {
//            title : "汽车"
//        },
//        sports : {
//            title : "体育"
//        },
//        finance : {
//            title : "财经"
//        },
//        fresh : {
//            title : "新鲜事"
//        },
//        blog : {
//            title : "博客"
//        },
//        women : {
//            title : "女性"
//        },
//        house : {
//            title : "房产"
//        }
//    };
//    var o = ["chief", "ent", "social", "cars", "sports", "finance", "fresh", "blog", "women", "house"];
//    var r = {
//        hasGroup : false,
//        groupSize : 1,
//        groupStripe : true,
//        maxLength : 7
//    };
//    var j = {
//        hasGroup : false,
//        groupSize : 1,
//        groupStripe : true,
//        maxLength : 1
//    };
//    var d = {
//        hasGroup : false,
//        groupSize : 1,
//        groupStripe : true,
//        maxLength : 2
//    };
//    var u = $("#news-tabs");
//    var f = $("#news .module-body");
//    var c;
//    var q;
//    var g;
//    var m = [];
//    i.listen("hook-news", function (w) {
//            m = w;
//        });
//    i.listen("start-news", function (w) {
//            $.each(o, function (y, z) {
//                    var x = s[z];
//                    $('<a href="#" data-name="t' + x.title + '"></a>').html("<span><span class='tab-border-left'></span><span class='tab-content'>" + x.title + "</span><span class='tab-border-right'></span></span>").appendTo(u);
//                    $('<div class="panel" data-name="' + x.title + '-0"></div>').html('<div class="loading">内容载入中...</div>').appendTo(f);
//                });
//            u.find("a:eq(0) span").addClass("first-tab");
//            c = f.find(".panel");
//            u.tabs("#news .module-body .panel", {
//                    onClick : function (y, x) {
//                        e(x);
//                    }
//                });
//            q = u.data("tabs");
//            q.hoverClick();
//            if (!g) {
//                g = setTimeout(h, 5000);
//            }
//            u.find("a").bind("click", function () {
//                    i.notify("show-widget", {
//                            id : "news",
//                            index : q.getIndex()
//                        });
//                });
//            $("#news").bind("mouseenter", function (x) {
//                    k();
//                });
//        });
//    function v(w) {
//        return w.replace(/&amp;/g, "&").replace(/&gt;/g, ">").replace(/&lt;/g, "<").replace(/&quot;/g, '"').replace(/&#039;/g, "'").replace(/&nbsp;/g, " ").replace(/<\/?[^>]+>/gi, "");
//    }
//    function b(w) {
//        $(w.news.list).each(function (y, x) {
//                $(x.headlines).each(function (A, z) {
//                        z.link = z.link.replace(/^http:\/\/sina.allyes.com\/main\/adfclick\?.*url=(.*)$/, "$1");
//                        z.title = v(z.title);
//                    });
//                $(x.items).each(function (z, A) {
//                        A.link = A.link.replace(/^http:\/\/sina.allyes.com\/main\/adfclick\?.*url=(.*)$/, "$1");
//                        A.title = v(A.title);
//                    });
//            });
//    }
//    i.listen("start-news-catogery", function (A) {
//            var F = $.grep(A.news.list[0].items, function (J, I) {
//                    return(J.type && J.type == "image");
//                });
//            var z = $.grep(A.news.list[0].items, function (J, I) {
//                    return(J.type == undefined);
//                });
//            for (var B = 0; B < F.length; B++) {
//                z.push(F[B]);
//            }
//            A.news.list[0].items = z;
//            $(m).each(function (I, J) {
//                    if (J.category == A.category) {
//                        A = $.extend(true, A, J);
//                    }
//                });
//            b(A);
//            var G = A.category;
//            r.statistics = G;
//            r.t = s[G].title;
//            j.statistics = G;
//            j.t = s[G].title;
//            d.statistics = G;
//            d.t = s[G].title;
//            s[G].data = A;
//            i.notify("response-news-catogery", {
//                    key : G,
//                    data : s[G].data
//                });
//            var x = A.news.list[0].headlines;
//            var H = l(A.news.list[0].items);
//            var D = $.inArray(G, o);
//            var w = $(c[D]);
//            w.html('<div class="headline"></div><div class="list links-block"></div><div class="thumb-list"></div>');
//            var E = w.find(".headline");
//            var C = w.find(".list");
//            var y = w.find(".thumb-list");
//            if (w) {
//                a(E, x, j);
//                p(y, H[1], d);
//                C.links(H[0], r);
//                w.attr("status", "completed");
//            }
//            r.statistics = r.t = null;
//        });
//    i.listen("request-news", function (w) {
//            i.notify("response-news", {
//                    res : s,
//                    order : o,
//                    index : q.getIndex()
//                });
//        });
//    i.listen("request-news-catogery", function (w) {
//            if (s[w].data) {
//                i.notify("response-news-catogery", {
//                        key : w,
//                        data : s[w].data
//                    });
//            } else {
//                i.request({
//                        type : "script",
//                        url : "/data/news_" + w + ".js"
//                    }, function () {
//                    });
//            }
//        });
//    $("#news-btn-more").click(function (w) {
//            i.notify("show-widget", {
//                    id : "news",
//                    index : q.getIndex()
//                });
//            w.preventDefault();
//        });
//    function l(y) {
//        var z = [];
//        var w = [];
//        for (var x = 0; x < y.length; x++) {
//            if (y[x].type == "image") {
//                w.push(y[x]);
//            } else {
//                z.push(y[x]);
//            }
//        }
//        return[z, w];
//    }
//    function e(x) {
//        var y = o[x];
//        var w = $(c[x]);
//        if (w.attr("status")) {
//            return;
//        }
//        w.attr("status", "building");
//        i.notify("request-news-catogery", y);
//    }
//    function h() {
//        k();
//        if (q.getIndex() >= q.getTabs().length - 1) {
//            q.click(0);
//        } else {
//            q.next();
//            g = setTimeout(h, 5000);
//        }
//    }
//    function k() {
//        if (g) {
//            clearTimeout(g);
//            g = null;
//        }
//    }
//    var p = function (A, z, x) {
//        if (A.length < 1) {
//            return;
//        }
//        x = $.extend({
//            }, $.tools.links.conf, x);
//        A.empty();
//        var B = A;
//        var w = B;
//        var y = 0;
//        $.each(z, function (D, C) {
//                if (x.maxLength > 1 && y >= x.maxLength) {
//                    return;
//                }
//                $.extend(C, {
//                        statistics : x.statistics,
//                        t : x.t
//                    });
//                y++;
//                var E = n(C);
//                w.append(E);
//            });
//    };
//    var a = function (z, y, x) {
//        if (z.length < 1 || !y) {
//            return;
//        }
//        x = $.extend({
//            }, $.tools.links.conf, x);
//        z.empty();
//        var A = z;
//        var w = A;
//        $.each(y, function (C, B) {
//                if (x.maxLength > 1 && C >= x.maxLength) {
//                    return;
//                }
//                $.extend(B, {
//                        statistics : x.statistics,
//                        t : x.t
//                    });
//                if (B.img) {
//                    var D = t(B);
//                    w.append(D);
//                }
//            });
//    };
//    function t(w) {
//        var z = $('<a target="_blank"></a>').attr("class", "img").attr("title", w.title).attr("href", w.link).attr("v", w.t).attr("statistics", w.statistics);
//        var x = $("<img></img>").attr("src", w.img).attr("width", "200").attr("height", "120");
//        var y = $("<span></span>").text(w.title);
//        z.append(x);
//        z.append(y);
//        return z;
//    }
//    function n(w) {
//        var z = $('<a target="_blank"></a>').attr("title", w.title).attr("href", w.link).attr("v", w.t).attr("statistics", w.statistics);
//        var x = $("<img></img>");
//        x.attr("src", w.img).attr("width", "80").attr("height", "60");
//        var y = $("<span></span>").html(w.title);
//        z.append(x);
//        z.append(y);
//        return z;
//    }
//});
//Core.reg("tools", function (d) {
//		var e = [{
//				title : "天气预报",
//				link : "http://www.weather.com.cn/"
//			}, {
//				title : "福利彩票",
//				link : "http://www.zhcw.com/"
//			}, {
//				title : "体育彩票",
//				link : "http://www.lottery.gov.cn/"
//			}, {
//				title : "酒店查询",
//				link : "http://www.qunar.com/twell/redirect.jsp?url=http://hotel.qunar.com?dh=ayo1"
//			}, {
//				title : "股票查询",
//				link : "http://summary.jrj.com.cn/"
//			}, {
//				title : "外汇牌价",
//				link : "http://www.boc.cn/sourcedb/whpj/"
//			}, {
//				title : "网速测试",
//				link : "http://www.linkwan.com/gb/broadmeter/SpeedAuto/"
//			}, {
//				title : "周公解梦",
//				link : "http://www.51jiemeng.com/"
//			}, {
//				title : "星座运程",
//				link : "http://www.13393.com/u/xingzuo/index.htm"
//			}, {
//				title : "手机归属",
//				link : "http://www.ip138.com/"
//			}, {
//				title : "快递查询",
//				link : "http://www.13393.com/kuaidi.htm"
//			}, {
//				title : "交通违章",
//				link : "http://auto.sohu.com/s2004/weizhangchaxun.shtml"
//			}, {
//				title : "热门团购",
//				link : "http://i.maxthon.cn/tuan/index.htm"
//			}, {
//				title : "汽车报价",
//				link : "http://car.autohome.com.cn/"
//			}, {
//				title : "手机型号",
//				link : "http://www.3533.com/phone/"
//			}, {
//				title : "淘宝特卖",
//				link : "http://pindao.huoban.taobao.com/channel/onSale.htm?pid=mm_12431063_2220385_8721096"
//			}, {
//				title : "在线翻译",
//				link : "http://translate.google.com.hk/"
//			}, {
//				title : "身份证",
//				link : "http://qq.ip138.com/idsearch/"
//			}
//		];
//		d.listen("hook-tools", function (l) {
//				if (l.resources) {
//					for (var k in l.resources) {
//						e[k] = l.resources[k];
//					}
//				}
//				if (l.order) {
//					var j = l.order,
//					h = [],
//					g = j.length;
//					for (var k = 0; k < g; k++) {
//						h[k] = e[j[k + 1]];
//					}
//					l.order.sort();
//					for (var k = 0; k < g; k++) {
//						e[j[k + 1]] = h[k];
//					}
//				}
//			});
//		$.each(e, function (g, h) {
//				h.link = decodeURIComponent(h.link);
//			});
//		var b = d.host();
//		var f = $("#toolsList");
//		var a = function () {
//			f.links(e, {
//					hasGroup : true,
//					groupSize : 9,
//					groupStripe : true,
//					maxLength : 18
//				});
//			$("#tools .wrap .panel:first").show();
//			var h = new Date();
//			var j = h.getFullYear();
//			var k = h.getMonth() + 1;
//			var g = h.getDate();
//			var i;
//			if (k < 10) {
//				k = "0" + k;
//			}
//			if (g < 10) {
//				g = "0" + g;
//			}
//			i = j + "-" + k + "-" + g;
//			$("#flightTime").val(i);
//		};
//		var c = function () {
//			$(".btn-more", b).click(function (g) {
//					d.notify("show-widget", {
//							id : "tools"
//						});
//					g.preventDefault();
//				});
//			$(".tabs", b).tabs("#tools div.panel");
//			$(".btn", b).hover(function () {
//					$(this).addClass("btn-hover");
//				}, function () {
//					$(this).removeClass("btn-hover");
//				});
//			$("#fromCity,#toCity").focus(function () {
//					var g = this;
//					window.setTimeout(function () {
//							$(g).select();
//						}, 0);
//				});
//			$("#trainStart,#trainEnd").focus(function () {
//					$(this).attr("data-default", this.defaultValue);
//					if (this.value === $(this).attr("data-default")) {
//						$(this).removeClass("defaultText");
//						this.value = "";
//					}
//				}).blur(function () {
//					if (this.value === "" || this.value == $(this).attr("data-default")) {
//						this.value = $(this).attr("data-default");
//						$(this).addClass("defaultText");
//					}
//				});
//			$("#train").submit(function (i) {
//					var k = $("#trainStart").val();
//					var j = $("#trainStart").attr("defaultValue");
//					var h = $("#trainEnd").val();
//					var g = $("#trainEnd").attr("defaultValue");
//					if (k === j || h === g) {
//						window.open($(this).attr("action"));
//						i.preventDefault();
//					}
//				});
//			$("#mobile-no").focus(function () {
//					if ($(this).hasClass("empty")) {
//						$(this).val("");
//						$(this).removeClass("empty");
//					}
//				}).blur(function () {
//					if ($.trim($(this).val()) == "") {
//						$(this).addClass("empty");
//						$(this).val("请输入手机号");
//					}
//				});
//			$("#mobile-form").submit(function () {
//					if ($("#mobile-no").hasClass("empty")) {
//						$("#mobile-no").val("");
//						setTimeout(function () {
//								$("#mobile-no").val("请输入手机号");
//							}, 500);
//					}
//				});
//		};
//		d.listen("start-tools", function () {
//				a();
//				c();
//			});
//	});
//Core.reg("service-email", function (a) {
//		a.listen("start-service-email", function (f) {
//				d();
//			});
//		$("#btn-login").hover(function () {
//				$(this).addClass("btn-hover");
//			}, function () {
//				$(this).removeClass("btn-hover");
//			});
//		function d() {
//			for (var g in e) {
//				var f = $("<option></option>", {
//						text : e[g].n,
//						value : g
//					});
//				$("#email-select").append(f);
//			}
//			$("#email-select").css("width", "100%");
//			$("#email-select").change(function () {
//					$("#email-reg").attr("href", e[$(this).val()].r);
//				});
//			$("#email-reg").attr({
//					href : e[$("#email-select").val()].r
//				});
//			$("#email-login").click(function (h) {
//					a.statistic({
//							n : "g2",
//							m : "g",
//							v : $("#email-select").val()
//						});
//					c($("#email-select").val(), $("#email-name").val(), $("#email-password").val());
//					$("#email-password").val("");
//				});
//		}
//		function c(h, m, f) {
//			var g = $("#email-submit-layer");
//			var k = $("#email-form");
//			if (g.length > 0) {
//				g.remove();
//			}
//			g = $("<div id='email-submit-layer' style='display: none;'></div>");
//			k.attr("action", e[h].f.a);
//			var j = e[h].f.p;
//			for (var i in j) {
//				var l = $("<input />", {
//						name : i,
//						type : "hidden",
//						value : b(j[i], m, f)
//					});
//				g.append(l);
//			}
//			k.append(g);
//		}
//		function b(h, j, f) {
//			var g = /\{USR\}/;
//			var i = /\{PWD\}/;
//			if (g.test(h)) {
//				return h.replace(g, j);
//			}
//			if (i.test(h)) {
//				return h.replace(i, f);
//			}
//			return h;
//		}
//		var e = {
//			_163 : {
//				n : "@163.com \u7f51\u6613",
//				r : "http://reg.163.com/reg/reg0_new.jsp?product=urs",
//				f : {
//					a : "http://reg.163.com/CheckUser.jsp",
//					p : {
//						username : "{USR}",
//						password : "{PWD}",
//						url : "http://fm163.163.com/coremail/fcg/ntesdoor2?lightweight=1&verifycookie=1&language=-1&style=15",
//						enterVip : ""
//					}
//				}
//			},
//			_126 : {
//				n : "@126.com \u7f51\u6613",
//				r : "http://reg.163.com/reg/reg0_new.jsp?product=urs",
//				f : {
//					a : "http://reg.163.com/logins.jsp",
//					p : {
//						username : "{USR}@126.com",
//						password : "{PWD}",
//						domain : "126.com",
//						url : "http://entry.mail.126.com/cgi/ntesdoor?lightweight%3D1%26verifycookie%3D1%26language%3D0%26style%3D-1",
//						enterVip : ""
//					}
//				}
//			},
//			sina : {
//				n : "@sina.com \u65b0\u6d6a",
//				r : "http://mail.sina.com.cn/register/reg_freemail.php",
//				f : {
//					a : "http://mail.sina.com.cn/cgi-bin/login.cgi",
//					p : {
//						u : "{USR}",
//						psw : "{PWD}"
//					}
//				}
//			},
//			yahoo : {
//				n : "@yahoo.com.cn",
//				r : "http://member.cn.yahoo.com/cnreg/reginfo.html?id=10003",
//				f : {
//					a : "https://edit.bjs.yahoo.com/config/login",
//					p : {
//						login : "{USR}@yahoo.com.cn",
//						passwd : "{PWD}"
//					}
//				}
//			},
//			yahoocn : {
//				n : "@yahoo.cn",
//				r : "http://member.cn.yahoo.com/cnreg/reginfo.html?id=10003",
//				f : {
//					a : "https://edit.bjs.yahoo.com/config/login",
//					p : {
//						login : "{USR}@yahoo.cn",
//						passwd : "{PWD}"
//					}
//				}
//			},
//			gmail : {
//				n : "@gmail.com",
//				r : "https://www.google.com.hk/accounts/NewAccount?continue=http%3A%2F%2Fwww.google.com.hk%2F&hl=zh-CN",
//				f : {
//					a : "https://www.google.com.hk/accounts/ServiceLoginAuth",
//					p : {
//						Email : "{USR}",
//						Passwd : "{PWD}"
//					}
//				}
//			},
//			sohu : {
//				n : "@sohu.com \u641c\u72d0",
//				r : "https://passport.sohu.com/web/dispatchAction.action",
//				f : {
//					a : "http://passport.sohu.com/login.jsp",
//					p : {
//						er : "0",
//						appid : "1000",
//						vr : "1|1",
//						m : "{USR}",
//						loginid : "{USR}@sohu.com",
//						mpass : "{PWD}",
//						passwd : "{PWD}",
//						password : "{PWD}",
//						uName : "{USR}",
//						userName : "{USR}",
//						fl : "1",
//						ru : "http://login.mail.sohu.com/servlet/LoginServlet",
//						eru : "http://login.mail.sohu.com/login.jsp",
//						ct : "1173080990",
//						sg : "5082635c77272088ae7241ccdf7cf062"
//					}
//				}
//			},
//			tom : {
//				n : "@tom.com",
//				r : "http://bjcgi.tom.com/cgi-bin/tom_reg.cgi?rf=060704",
//				c : "utf-8",
//				f : {
//					a : "http://bjweb.163.net/cgi/163/login_pro.cgi",
//					p : {
//						user : "{USR}",
//						pass : "{PWD}"
//					}
//				}
//			},
//			_21cn : {
//				n : "@21cn.com",
//				r : "http://passport.21cn.com/register.jsp",
//				f : {
//					a : "http://passport.21cn.com/maillogin.jsp",
//					p : {
//						LoginName : "{USR}",
//						passwd : "{PWD}",
//						domainname : "21cn.com"
//					}
//				}
//			},
//			yeah : {
//				n : "@yeah.net",
//				r : "http://reg.163.com/reg0.shtml",
//				f : {
//					a : "https://reg.163.com/logins.jsp",
//					p : {
//						username : "{USR}@yeah.net",
//						password : "{PWD}",
//						domain : "yeah.net",
//						url : "http://entry.mail.yeah.net/cgi/ntesdoor?lightweight%3D1%26verifycookie%3D1%26style%3D-1"
//					}
//				}
//			}
//		};
//	});
//Core.reg("tools-special-lucky", function (d) {
//		var c = "http://www.go108.com.cn/astro/daily4maxthon.js.php";
//		var a = $("#star-select");
//		var e = ["all", "love", "work", "money"];
//		function b(f) {
//			$.each(astroDailyContent, function (g, h) {
//					if (h.astroName == f) {
//						$(e).each(function (i, j) {
//								$("#lucky-" + j).removeClass();
//								$("#lucky-" + j).addClass("star" + h[j]);
//							});
//						$("#lucky-url").attr("href", h.url);
//					}
//				});
//		}
//		$.getScript(c, function () {
//				a.change(function () {
//						$.cookieAdvanced("ch.new", "constellation", this.value);
//						b($("option:selected", this).text());
//						d.notify("widget_lucky_update", null);
//					});
//				if ($.cookieAdvanced("ch.new", "constellation")) {
//					a.val($.cookieAdvanced("ch.new", "constellation"));
//				}
//				b($("option:selected", a).text());
//			});
//		d.listen("mod_lucky_update", function () {
//				a.val($.cookieAdvanced("ch.new", "constellation"));
//			});
//	});
//Core.reg("services", function (g) {
//		var c = {
//			email : {
//				title : "邮箱",
//				module : "service-email"
//			},
//			lucky : {
//				title : "运势",
//				module : "service-lucky"
//			}//,
////			payment : {
////				title : "点卡",
////				module : "service-payment"
////			}
//		};
//		var d = ["email", "lucky"];//, "payment"
//		var j = $("#service-tabs");
//		var f = $("#services .module-body");
//		var i = null;
//		var k = null;
//		function e(n, l) {
//			var m = d[l];
//			if (c[m].status !== "completed") {
//				g.notify("start-service-" + m);
//				c[m].status = "completed";
//			}
//			$.cookieAdvanced("ch.new", "current-tool", m);
//			g.statistic({
//					n : "g0",
//					v : $.cookieAdvanced("ch.new", "current-tool"),
//					m : "g"
//				});
//		}
//		$.each(d, function (m, n) {
//				var l = c[n];
//				$('<a href="#"></a>').text(l.title).appendTo(j);
//			});
//		$(j.find("a")[0]).addClass("first-tab");
//		var b = $.cookieAdvanced("ch.new", "current-tool") || "";
//		var h = $.inArray(b, d);
//		var a = d[h];
//		if (!a) {
//			a = d[0];
//			h = 0;
//		}
//		i = f.find(".panel");
//		j.tabs("#services .module-body .panel", {
//				initialIndex : h,
//				onClick : e
//			});
//		k = j.data("tabs");
//		k.hoverClick();
//	});