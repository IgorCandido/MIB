#
# $Id: ssbg.spec,v 1.2 2008-04-01 16:13:57 carzanig Exp $
#
%define name	ssbg
%define ver	%version
%define rel	1

Summary:   Siena Synthetic Benchmark Generator
Name:      %{name}
Version:   %{ver}
Release:   %rel
License:   GPL
Group:     Applications/Internet
Source:    http://www.cs.colorado.edu/serl/siena/forwarding/%{name}-%{ver}.tar.gz
URL:       http://www.cs.colorado.edu/serl/siena/forwarding/
Packager:  CU Software Engineering Research Lab (SERL) <serl@cs.colorado.edu>

BuildRoot: %{_topdir}/rpmroot

%description 

The Siena Synthetic Benchmark Generator creates workloads for
evaluating publish / subscribe systems.

%prep
rm -rf $RPM_BUILD_ROOT

%setup 

%build

CFLAGS="$RPM_OPT_FLAGS" ./configure "--prefix=$RPM_BUILD_ROOT/usr" --disable-dependency-tracking
make all

%install

make install docdir=$RPM_BUILD_DIR/%{name}-%{ver}

%clean
rm -rf $RPM_BUILD_ROOT

%files
%defattr(-,root,root,755)
%doc AUTHORS COPYING INSTALL NEWS README ChangeLog
%attr(644,root,root) /usr/include/siena/*
%attr(755,root,root) /usr/lib/lib*.so*
%attr(644,root,root) /usr/lib/*.*a
%attr(755,root,root) /usr/bin/*

%post
ldconfig

%postun
ldconfig

