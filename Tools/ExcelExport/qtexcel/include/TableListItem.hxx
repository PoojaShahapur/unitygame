#ifndef TABLELISTITEM_H
#define TABLELISTITEM_H

#include <QWidget>
#include "CTask.hxx"
#include "ui_TableListItem.h"

namespace Ui
{
	class TableListItem;
}

class TableListItem : public QWidget
{
	Q_OBJECT
public:
	Ui::TableListItem *m_ui;

private:
	Table* m_table;

public slots:
	void onChkClk();

public:
	explicit TableListItem(QWidget *parent);
	void setTable(Table* table);

signals:
	public slots :
};

#endif // TABLELISTITEM_H