/////////////////////////////////////////////////////////////////////////////
// olectl.h     OLE Control interfaces
//              OLE Version 2.0
//              Copyright (c) Microsoft Corporation. All rights reserved.
/////////////////////////////////////////////////////////////////////////////

#ifndef _OLECTL_H_
#define _OLECTL_H_

#if !defined(__MKTYPLIB__) && !defined(__midl)

#if !defined(RC_INVOKED)
#pragma pack(push,8)
#endif

//#include <ocidl.h>
#include <wtypes.h>

#if !defined(INITGUID)

:DEFINE_GUID(IID_IPropertyFrame,0xB196B28A,0xBAB4,0x101A,0xB6,0x9C,0x00,0xAA,0x00,0x34,0x1D,0x07);

//  Class IDs for property sheet implementations
:DEFINE_GUID(CLSID_CFontPropPage,0x0be35200,0x8f91,0x11ce,0x9d,0xe3,0x00,0xaa,0x00,0x4b,0xb8,0x51);
:DEFINE_GUID(CLSID_CColorPropPage,0x0be35201,0x8f91,0x11ce,0x9d,0xe3,0x00,0xaa,0x00,0x4b,0xb8,0x51);
:DEFINE_GUID(CLSID_CPicturePropPage,0x0be35202,0x8f91,0x11ce,0x9d,0xe3,0x00,0xaa,0x00,0x4b,0xb8,0x51);

//  Class IDs for persistent property set formats
:DEFINE_GUID(CLSID_PersistPropset,0xfb8f0821,0x0164,0x101b,0x84,0xed,0x08,0x00,0x2b,0x2e,0xc7,0x13);
:DEFINE_GUID(CLSID_ConvertVBX,0xfb8f0822,0x0164,0x101b,0x84,0xed,0x08,0x00,0x2b,0x2e,0xc7,0x13);

//  Class ID for standard implementations of IFont and IPicture
:DEFINE_GUID(CLSID_StdFont,0x0be35203,0x8f91,0x11ce,0x9d,0xe3,0x00,0xaa,0x00,0x4b,0xb8,0x51);
:DEFINE_GUID(CLSID_StdPicture,0x0be35204,0x8f91,0x11ce,0x9d,0xe3,0x00,0xaa,0x00,0x4b,0xb8,0x51);

//  GUIDs for standard types
:DEFINE_GUID(GUID_HIMETRIC,0x66504300,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_COLOR,0x66504301,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_XPOSPIXEL,0x66504302,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_YPOSPIXEL,0x66504303,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_XSIZEPIXEL,0x66504304,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_YSIZEPIXEL,0x66504305,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_XPOS,0x66504306,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_YPOS,0x66504307,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_XSIZE,0x66504308,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_YSIZE,0x66504309,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);

#if !defined( __CGUID_H__ )
:DEFINE_GUID(GUID_TRISTATE,0x6650430A,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
#endif

:DEFINE_GUID(GUID_OPTIONVALUEEXCLUSIVE,0x6650430B,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_CHECKVALUEEXCLUSIVE,0x6650430C,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_FONTNAME,0x6650430D,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_FONTSIZE,0x6650430E,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_FONTBOLD,0x6650430F,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_FONTITALIC,0x66504310,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_FONTUNDERSCORE,0x66504311,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_FONTSTRIKETHROUGH,0x66504312,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
:DEFINE_GUID(GUID_HANDLE,0x66504313,0xBE0F,0x101A,0x8B,0xBB,0x00,0xAA,0x00,0x30,0x0C,0xAB);
#endif // INITGUID

/////////////////////////////////////////////////////////////////////////////
// OCPFIPARAMS structure - parameters for OleCreatePropertyFrameIndirect

#define DISPID long

struct OCPFIPARAMS
{
  ULONG cbStructSize;
  HWND hWndOwner;
  int x;
  int y;
  dword lpszCaption;
  ULONG cObjects;
  dword lplpUnk;
  ULONG cPages;
  dword lpPages;
  LCID lcid;
  DISPID dispidInitialProperty;
};

/////////////////////////////////////////////////////////////////////////////
// FONTDESC structure
//#define FONTSIZE(n) { n##0000, 0 }

struct FONTDESC
{
  UINT cbSizeofstruct;
  dword lpstrName;
  CY cySize;
  SHORT sWeight;
  SHORT sCharset;
  BOOL fItalic;
  BOOL fUnderline;
  BOOL fStrikethrough;
};

/////////////////////////////////////////////////////////////////////////////
// PICTDESC structure

#define PICTYPE_UNINITIALIZED -1
#define PICTYPE_NONE            0
#define PICTYPE_BITMAP          1
#define PICTYPE_METAFILE        2
#define PICTYPE_ICON            3
#ifdef _WIN32
#define PICTYPE_ENHMETAFILE     4
#endif

struct PICTDESC
{
  UINT cbSizeofstruct;
  UINT picType;
  union
  {
    struct
    {
      HBITMAP   hbitmap;        // Bitmap
      HPALETTE  hpal;           // Accompanying palette
    }bmp;
    struct
    {
      HMETAFILE hmeta;          // Metafile
      int       xExt;
      int       yExt;           // Extent
    }wmf;
    struct
    {
      HICON hicon;              // Icon
    }icon;
#ifdef _WIN32
     struct
     {
       HENHMETAFILE hemf;        // Enhanced Metafile
     }emf;
#endif
  };
};

/////////////////////////////////////////////////////////////////////////////
// Typedefs for standard scalar types

#define OLE_XPOS_PIXELS      long
#define OLE_YPOS_PIXELS      long
#define OLE_XSIZE_PIXELS     long
#define OLE_YSIZE_PIXELS     long
#define OLE_XPOS_CONTAINER  float
#define OLE_YPOS_CONTAINER  float
#define OLE_XSIZE_CONTAINER float
#define OLE_YSIZE_CONTAINER float
enum OLE_TRISTATE{
	triUnchecked = 0,
	triChecked = 1,
	triGray = 2
};
#define OLE_OPTEXCLUSIVE      VARIANT_BOOL
#define OLE_CANCELBOOL        VARIANT_BOOL
#define OLE_ENABLEDEFAULTBOOL VARIANT_BOOL

/////////////////////////////////////////////////////////////////////////////
// FACILITY_CONTROL status codes
#ifndef FACILITY_CONTROL
#define FACILITY_CONTROL 0xa
#endif

#define STD_CTL_SCODE(n) MAKE_SCODE(SEVERITY_ERROR,FACILITY_CONTROL,n)
#define CTL_E_ILLEGALFUNCTIONCALL       0x800A0005
#define CTL_E_OVERFLOW                  0x800A0006
#define CTL_E_OUTOFMEMORY               0x800A0007
#define CTL_E_DIVISIONBYZERO            0x800A000B
#define CTL_E_OUTOFSTRINGSPACE          0x800A000E
#define CTL_E_OUTOFSTACKSPACE           0x800A001C
#define CTL_E_BADFILENAMEORNUMBER       0x800A0034
#define CTL_E_FILENOTFOUND              0x800A0035
#define CTL_E_BADFILEMODE               0x800A0036
#define CTL_E_FILEALREADYOPEN           0x800A0037
#define CTL_E_DEVICEIOERROR             0x800A0039
#define CTL_E_FILEALREADYEXISTS         0x800A003A
#define CTL_E_BADRECORDLENGTH           0x800A003B
#define CTL_E_DISKFULL                  0x800A003D
#define CTL_E_BADRECORDNUMBER           0x800A003F
#define CTL_E_BADFILENAME               0x800A0040
#define CTL_E_TOOMANYFILES              0x800A0043
#define CTL_E_DEVICEUNAVAILABLE         0x800A0044
#define CTL_E_PERMISSIONDENIED          0x800A0046
#define CTL_E_DISKNOTREADY              0x800A0047
#define CTL_E_PATHFILEACCESSERROR       0x800A004B
#define CTL_E_PATHNOTFOUND              0x800A004C
#define CTL_E_INVALIDPATTERNSTRING      0x800A005D
#define CTL_E_INVALIDUSEOFNULL          0x800A005E
#define CTL_E_INVALIDFILEFORMAT         0x800A0141
#define CTL_E_INVALIDPROPERTYVALUE      0x800A017C
#define CTL_E_INVALIDPROPERTYARRAYINDEX 0x800A017D
#define CTL_E_SETNOTSUPPORTEDATRUNTIME  0x800A017E
#define CTL_E_SETNOTSUPPORTED           0x800A017F
#define CTL_E_NEEDPROPERTYARRAYINDEX    0x800A0181
#define CTL_E_SETNOTPERMITTED           0x800A0183
#define CTL_E_GETNOTSUPPORTEDATRUNTIME  0x800A0189
#define CTL_E_GETNOTSUPPORTED           0x800A018A
#define CTL_E_PROPERTYNOTFOUND          0x800A01A6
#define CTL_E_INVALIDCLIPBOARDFORMAT    0x800A01CC
#define CTL_E_INVALIDPICTURE            0x800A01E1
#define CTL_E_PRINTERERROR              0x800A01E2
#define CTL_E_CANTSAVEFILETOTEMP        0x800A02DF
#define CTL_E_SEARCHTEXTNOTFOUND        0x800A02E8
#define CTL_E_REPLACEMENTSTOOLONG       0x800A02EA

#define CUSTOM_CTL_SCODE(n) MAKE_SCODE(SEVERITY_ERROR,FACILITY_CONTROL,n)
#define CTL_E_CUSTOM_FIRST              0x800A0258

/////////////////////////////////////////////////////////////////////////////
// IConnectionPoint status codes
#define CONNECT_E_FIRST    0x80040200
#define CONNECT_E_LAST     0x8004020F
#define CONNECT_S_FIRST    0x40200
#define CONNECT_S_LAST     0x4020F

#define CONNECT_E_NOCONNECTION      CONNECT_E_FIRST+0
// there is no connection for this connection id

#define CONNECT_E_ADVISELIMIT       CONNECT_E_FIRST+1
// this implementation's limit for advisory connections has been reached

#define CONNECT_E_CANNOTCONNECT     CONNECT_E_FIRST+2
// connection attempt failed

#define CONNECT_E_OVERRIDDEN        CONNECT_E_FIRST+3
// must use a derived interface to connect

/////////////////////////////////////////////////////////////////////////////
// DllRegisterServer/DllUnregisterServer status codes

#define SELFREG_E_FIRST    0x80040200
#define SELFREG_E_LAST     0x8004020F
#define SELFREG_S_FIRST    0x40200
#define SELFREG_S_LAST     0x4020F

#define SELFREG_E_TYPELIB           SELFREG_E_FIRST+0
// failed to register/unregister type library

#define SELFREG_E_CLASS             SELFREG_E_FIRST+1
// failed to register/unregister class

/////////////////////////////////////////////////////////////////////////////
// IPerPropertyBrowsing status codes

#define PERPROP_E_FIRST    0x80040200
#define PERPROP_E_LAST     0x8004020F
#define PERPROP_S_FIRST    0x40200
#define PERPROP_S_LAST     0x4020F

#define PERPROP_E_NOPAGEAVAILABLE   PERPROP_E_FIRST
// no page available for requested property

/////////////////////////////////////////////////////////////////////////////
// OLEMISC_ constants (they are also defined in the latest oleidl.h)

#define OLEMISC_INVISIBLEATRUNTIME  0x00000400L
#define OLEMISC_ALWAYSRUN           0x00000800L
#define OLEMISC_ACTSLIKEBUTTON      0x00001000L
#define OLEMISC_ACTSLIKELABEL       0x00002000L
#define OLEMISC_NOUIACTIVATE        0x00004000L
#define OLEMISC_ALIGNABLE           0x00008000L
#define OLEMISC_SIMPLEFRAME         0x00010000L
#define OLEMISC_SETCLIENTSITEFIRST  0x00020000L
#define OLEMISC_IMEMODE             0x00040000L

/////////////////////////////////////////////////////////////////////////////
// OLEIVERB_ constants

#ifndef OLEIVERB_PROPERTIES
#define OLEIVERB_PROPERTIES -7L
#endif

/////////////////////////////////////////////////////////////////////////////
// Variant type (VT_) tags for property sets

#define VT_STREAMED_PROPSET 73  //       [P]  Stream contains a property set
#define VT_STORED_PROPSET   74  //       [P]  Storage contains a property set
#define VT_BLOB_PROPSET     75  //       [P]  Blob contains a property set
#define VT_VERBOSE_ENUM     76  //       [P]  Enum value with text string

/////////////////////////////////////////////////////////////////////////////
// Variant type (VT_) tags that are just aliases for others

#define VT_COLOR            VT_I4
#define VT_XPOS_PIXELS      VT_I4
#define VT_YPOS_PIXELS      VT_I4
#define VT_XSIZE_PIXELS     VT_I4
#define VT_YSIZE_PIXELS     VT_I4
#define VT_XPOS_HIMETRIC    VT_I4
#define VT_YPOS_HIMETRIC    VT_I4
#define VT_XSIZE_HIMETRIC   VT_I4
#define VT_YSIZE_HIMETRIC   VT_I4
#define VT_TRISTATE         VT_I2
#define VT_OPTEXCLUSIVE     VT_BOOL
#define VT_FONT             VT_DISPATCH
#define VT_PICTURE          VT_DISPATCH

#ifdef _WIN32
#define VT_HANDLE           VT_I4
#else
#define VT_HANDLE           VT_I2
#endif

/////////////////////////////////////////////////////////////////////////////
// Reflected Window Message IDs

#define OCM__BASE           WM_USER+0x1c00
#define OCM_COMMAND         OCM__BASE + WM_COMMAND

#ifdef _WIN32
#define OCM_CTLCOLORBTN     OCM__BASE + WM_CTLCOLORBTN
#define OCM_CTLCOLOREDIT    OCM__BASE + WM_CTLCOLOREDIT
#define OCM_CTLCOLORDLG     OCM__BASE + WM_CTLCOLORDLG
#define OCM_CTLCOLORLISTBOX OCM__BASE + WM_CTLCOLORLISTBOX
#define OCM_CTLCOLORMSGBOX  OCM__BASE + WM_CTLCOLORMSGBOX
#define OCM_CTLCOLORSCROLLBAR   OCM__BASE + WM_CTLCOLORSCROLLBAR
#define OCM_CTLCOLORSTATIC  OCM__BASE + WM_CTLCOLORSTATIC
#else
#define OCM_CTLCOLOR        OCM__BASE + WM_CTLCOLOR
#endif

#define OCM_DRAWITEM        OCM__BASE + WM_DRAWITEM
#define OCM_MEASUREITEM     OCM__BASE + WM_MEASUREITEM
#define OCM_DELETEITEM      OCM__BASE + WM_DELETEITEM
#define OCM_VKEYTOITEM      OCM__BASE + WM_VKEYTOITEM
#define OCM_CHARTOITEM      OCM__BASE + WM_CHARTOITEM
#define OCM_COMPAREITEM     OCM__BASE + WM_COMPAREITEM
#define OCM_HSCROLL         OCM__BASE + WM_HSCROLL
#define OCM_VSCROLL         OCM__BASE + WM_VSCROLL
#define OCM_PARENTNOTIFY    OCM__BASE + WM_PARENTNOTIFY
#define OCM_NOTIFY          OCM__BASE + WM_NOTIFY

/////////////////////////////////////////////////////////////////////////////
// Self-registration APIs (to be implemented by server DLL)
extern WINAPI"olepro32.dll"{
	DllRegisterServer(void);
	DllUnregisterServer(void);

/////////////////////////////////////////////////////////////////////////////
// Property frame APIs
	OleCreatePropertyFrame(HWND hwndOwner,UINT x,UINT y,dword lpszCaption,ULONG cObjects,dword ppUnk,ULONG cPages,
    dword pPageClsID,LCID lcid,DWORD dwReserved,LPVOID pvReserved);
	OleCreatePropertyFrameIndirect(dword lpParams);

/////////////////////////////////////////////////////////////////////////////
// Standard type APIs
	OleTranslateColor(dword clr,HPALETTE hpal,dword lpcolorref);
	OleCreateFontIndirect(dword lpFontDesc,dword riid,dword lplpvObj);
	OleCreatePictureIndirect(dword lpPictDesc,dword riid,BOOL fOwn,dword lplpvObj);
	OleLoadPicture(dword lpstream,LONG lSize,BOOL fRunmode,dword riid,dword lplpvObj);
}
extern WINAPI"oleaut32.dll"{
	OleLoadPictureEx(dword lpstream,LONG lSize,BOOL fRunmode,
    dword riid,DWORD xSizeDesired,DWORD ySizeDesired,DWORD dwFlags,dword lplpvObj);
	OleLoadPicturePath(dword szURLorPath,dword punkCaller,DWORD dwReserved,
		dword clrReserved,dword riid,dword  ppvRet);
//	OleLoadPictureFile(VARIANT varFileName,dword lplpdispPicture);
//	OleLoadPictureFileEx(VARIANT varFileName,DWORD xSizeDesired,DWORD ySizeDesired,DWORD dwFlags,dword lplpdispPicture);
	OleSavePictureFile(dword lpdispPicture,BSTR bstrFileName);
	HCURSOR OleIconToCursor(HINSTANCE hinstExe,HICON hIcon);
}
#define LP_DEFAULT      0x00
#define LP_MONOCHROME	0x01
#define LP_VGACOLOR     0x02
#define LP_COLOR        0x04

#if !defined(_MAC) && !defined(RC_INVOKED)
#pragma pack(pop)
#endif

#endif // !(defined(__MKTYPLIB__) && !defined(__midl))

/////////////////////////////////////////////////////////////////////////////
//  Standard dispatch ID constants
#define DISPID_AUTOSIZE                 -500
#define DISPID_BACKCOLOR                -501
#define DISPID_BACKSTYLE                -502
#define DISPID_BORDERCOLOR              -503
#define DISPID_BORDERSTYLE              -504
#define DISPID_BORDERWIDTH              -505
#define DISPID_DRAWMODE                 -507
#define DISPID_DRAWSTYLE                -508
#define DISPID_DRAWWIDTH                -509
#define DISPID_FILLCOLOR                -510
#define DISPID_FILLSTYLE                -511
#define DISPID_FONT                     -512
#define DISPID_FORECOLOR                -513
#define DISPID_ENABLED                  -514
#define DISPID_HWND                     -515
#define DISPID_TABSTOP                  -516
#define DISPID_TEXT                     -517
#define DISPID_CAPTION                  -518
#define DISPID_BORDERVISIBLE            -519
#define DISPID_APPEARANCE               -520
#define DISPID_MOUSEPOINTER             -521
#define DISPID_MOUSEICON                -522
#define DISPID_PICTURE                  -523
#define DISPID_VALID                    -524
#define DISPID_READYSTATE               -525
#define DISPID_LISTINDEX                -526
#define DISPID_SELECTED                 -527
#define DISPID_LIST                     -528
#define DISPID_COLUMN                   -529
#define DISPID_LISTCOUNT                -531
#define DISPID_MULTISELECT              -532
#define DISPID_MAXLENGTH                -533
#define DISPID_PASSWORDCHAR             -534
#define DISPID_SCROLLBARS               -535
#define DISPID_WORDWRAP                 -536
#define DISPID_MULTILINE                -537
#define DISPID_NUMBEROFROWS             -538
#define DISPID_NUMBEROFCOLUMNS          -539
#define DISPID_DISPLAYSTYLE             -540
#define DISPID_GROUPNAME                -541
#define DISPID_IMEMODE                  -542
#define DISPID_ACCELERATOR              -543
#define DISPID_ENTERKEYBEHAVIOR         -544
#define DISPID_TABKEYBEHAVIOR           -545
#define DISPID_SELTEXT                  -546
#define DISPID_SELSTART                 -547
#define DISPID_SELLENGTH                -548
#define DISPID_REFRESH                  -550
#define DISPID_DOCLICK                  -551
#define DISPID_ABOUTBOX                 -552
#define DISPID_ADDITEM                  -553
#define DISPID_CLEAR                    -554
#define DISPID_REMOVEITEM               -555
#define DISPID_CLICK                    -600
#define DISPID_DBLCLICK                 -601
#define DISPID_KEYDOWN                  -602
#define DISPID_KEYPRESS                 -603
#define DISPID_KEYUP                    -604
#define DISPID_MOUSEDOWN                -605
#define DISPID_MOUSEMOVE                -606
#define DISPID_MOUSEUP                  -607
#define DISPID_ERROREVENT               -608
#define DISPID_READYSTATECHANGE         -609
#define DISPID_CLICK_VALUE              -610
#define DISPID_RIGHTTOLEFT              -611
#define DISPID_TOPTOBOTTOM              -612
#define DISPID_THIS                     -613
#define DISPID_AMBIENT_BACKCOLOR        -701
#define DISPID_AMBIENT_DISPLAYNAME      -702
#define DISPID_AMBIENT_FONT             -703
#define DISPID_AMBIENT_FORECOLOR        -704
#define DISPID_AMBIENT_LOCALEID         -705
#define DISPID_AMBIENT_MESSAGEREFLECT   -706
#define DISPID_AMBIENT_SCALEUNITS       -707
#define DISPID_AMBIENT_TEXTALIGN        -708
#define DISPID_AMBIENT_USERMODE         -709
#define DISPID_AMBIENT_UIDEAD           -710
#define DISPID_AMBIENT_SHOWGRABHANDLES  -711
#define DISPID_AMBIENT_SHOWHATCHING     -712
#define DISPID_AMBIENT_DISPLAYASDEFAULT -713
#define DISPID_AMBIENT_SUPPORTSMNEMONICS -714
#define DISPID_AMBIENT_AUTOCLIP         -715
#define DISPID_AMBIENT_APPEARANCE       -716
#define DISPID_AMBIENT_CODEPAGE         -725
#define DISPID_AMBIENT_PALETTE          -726
#define DISPID_AMBIENT_CHARSET          -727
#define DISPID_AMBIENT_TRANSFERPRIORITY -728
#define DISPID_AMBIENT_RIGHTTOLEFT      -732
#define DISPID_AMBIENT_TOPTOBOTTOM      -733
#define DISPID_Name                     -800
#define DISPID_Delete                   -801
#define DISPID_Object                   -802
#define DISPID_Parent                   -803

/////////////////////////////////////////////////////////////////////////////
// Dispatch ID constants for font and picture types
#define DISPID_FONT_NAME    0
#define DISPID_FONT_SIZE    2
#define DISPID_FONT_BOLD    3
#define DISPID_FONT_ITALIC  4
#define DISPID_FONT_UNDER   5
#define DISPID_FONT_STRIKE  6
#define DISPID_FONT_WEIGHT  7
#define DISPID_FONT_CHARSET 8
#define DISPID_FONT_CHANGED 9
#define DISPID_PICT_HANDLE  0
#define DISPID_PICT_HPAL    2
#define DISPID_PICT_TYPE    3
#define DISPID_PICT_WIDTH   4
#define DISPID_PICT_HEIGHT  5
#define DISPID_PICT_RENDER  6

#endif // _OLECTL_H_
